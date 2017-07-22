using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.IO;
using System;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

/** ********** 사전 세팅 *****************************************************************
  * Player Settings > Resolution and Presentation > Run in Background > true
  *
  * 
  */

namespace GM
{
    public class NetworkManager : MonoBehaviour
    {
        static Socket socket = null;
        public string address = "127.0.0.1";   // 주소, 서버 주소와 같게 할 것
        int port = 10000;               // 포트 번호, 서버포트와 같게 할 것
        byte[] buf = new byte[4096];
        int recvLen = 0;
        public bool isAdmin = false;

        public GameObject nowLoadingWindow;

        public string nickName;
        public List<SPlayerMove> v_user = new List<SPlayerMove>();

        public GameObject playerPrefs;

        public SoundManager _sound;
        static NetworkManager _instance;
        public static NetworkManager getInstance
        {
            get
            {
                return _instance;
            }
        }

        void Awake()
        {
            DontDestroyOnLoad(this);
            _instance = this;
        }

        void Update()
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                SendMsg("DEBUG");
            }
        }

        /**
         * @brief 서버에 접속 
         */
        public void Login()
        {
            nowLoadingWindow.SetActive(true);

            if (checkNetwork())
            {
                Logout();       // 이중 접속 방지

                IPAddress serverIP = IPAddress.Parse(address);
                int serverPort = Convert.ToInt32(port);
                socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.SendTimeout, 10000);      // 송신 제한시간 10초
                socket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReceiveTimeout, 10000);   // 수신 제한시간 10초

                // 서버가 닫혀 있을것을 대비하여 예외처리
                try
                {
                    socket.Connect(new IPEndPoint(serverIP, serverPort));
                    StartCoroutine("PacketProc");

                    nowLoadingWindow.SetActive(true);
                    SceneManager.LoadScene("InGame");
                    _sound.readyBGM();
                }
                catch (SocketException err)
                {
                    Debug.Log("서버가 닫혀있습니다. : " + err.ToString());
                    Logout();
                }
                catch (Exception ex)
                {
                    Debug.Log("ERROR 개반자에게 문의 : " + ex.ToString());
                    Logout();
                }
            }
            else
            {
                nowLoadingWindow.SetActive(false);
            }
        }

        /**
         * @brief 접속 종료 
         */
        public void Logout()
        {
            if (socket != null && socket.Connected)
                socket.Close();
            StopCoroutine("PacketProc");
        }

        /**
         * @brief 채팅
         * @param input 내용
         */
        public void Chat(InputField input)
        {
            SendMsg(string.Format("CHAT:{0}", input.text));
        }

        /**
         * @brief 접속 종료 
         * @param nickName 이름
         * @param pos 생성 위치
         * @param isPlayer 나 인가 아닌가
         */
        public void CreateUser(int idx, string nickName, Vector3 pos, MOVE_CONTROL moveC, bool isPlayer)
        {
            GameObject obj = Instantiate(playerPrefs, pos, Quaternion.identity) as GameObject;
            SPlayerMove player = obj.GetComponent<SPlayerMove>();

            player.myIdx = idx;
            player.nickName = nickName;
            player.isPlayer = isPlayer;
            player.myMove = moveC;

            v_user.Add(player);
        }

        /**
         * @brief 서버에게 패킷 전달
         * @param txt 패킷 내용
         */
        public void SendMsg(string txt)
        {
            try
            {
                if (socket != null && socket.Connected)
                {
                    byte[] buf = new byte[4096];

                    Buffer.BlockCopy(ShortToByte(Encoding.UTF8.GetBytes(txt).Length + 2), 0, buf, 0, 2);

                    Buffer.BlockCopy(Encoding.UTF8.GetBytes(txt), 0, buf, 2, Encoding.UTF8.GetBytes(txt).Length);

                    socket.Send(buf, Encoding.UTF8.GetBytes(txt).Length + 2, 0);
                }
            }
            catch (System.Exception e)
            {
                Debug.Log(e);
            }
        }

        /**
         * @brief 패킷 처리 업데이트
         */
        IEnumerator PacketProc()
        {
            while (true)
            {
                try
                {
                    if (socket.Connected)
                        if (socket.Available > 0)
                        {
                            byte[] buf = new byte[4096];
                            int nRead = socket.Receive(buf, socket.Available, 0);

                            if (nRead > 0)
                            {
                                Buffer.BlockCopy(buf, 0, this.buf, recvLen, nRead);
                                recvLen += nRead;

                                while (true)
                                {
                                    int len = BitConverter.ToInt16(this.buf, 0);

                                    if (len > 0 && recvLen >= len)
                                    {
                                        ParsePacket(len);
                                        recvLen -= len;
                                        Buffer.BlockCopy(this.buf, len, this.buf, 0, recvLen);
                                    }
                                    else
                                    {
                                        break;
                                    }
                                }
                            }
                        }
                }
                catch (Exception ex)
                {
                    Debug.Log(Encoding.UTF8.GetString(this.buf, 2, BitConverter.ToInt16(this.buf, 0) - 2));
                    Debug.Log(ex.ToString());
                }
                yield return null;
            }
        }

        /**
         * @brief 패킷 분석
         */
        public void ParsePacket(int len)
        {
            string msg = Encoding.UTF8.GetString(this.buf, 2, len - 2);
            string[] txt = msg.Split(':');

            if (txt[0].Equals("CONNECT"))
            {
                Debug.Log("Connected.");
                SendMsg(string.Format("LOGIN:{0}", nickName));
            }
            else if (txt[0].Equals("USER"))
            {
                // 기존 유저를 생성할때 호출됨
                /* nick, posX, posY, move_control, direction */
                CreateUser(int.Parse(txt[1]), txt[2], new Vector3(float.Parse(txt[3]), float.Parse(txt[4]), 0), (MOVE_CONTROL)int.Parse(txt[5]), false);
            }
            else if (txt[0].Equals("ADDUSER"))
            {
                // // 사람이 입장하기 전에 유저가 한명 이하라는 것은 본인이 방장임을 뜻함
                if (v_user.Count < 1) isAdmin = true;
                else isAdmin = false;

                // 새로운 유저를 생성할때 호출됨
                CreateUser(int.Parse(txt[1]), nickName, Vector3.zero, MOVE_CONTROL.STOP, true);
            }
            else if (txt[0].Equals("CHAT"))
            {
                SGameMng.I._chat.chat(string.Format("[{0}] : {1}", txt[1], txt[2]));
            }
            else if (txt[0].Equals("MOVE"))
            {
                int idx = int.Parse(txt[1]);
                for (int i = 0; i < v_user.Count; i++)
                {
                    if (v_user[i] != null)
                        if (v_user[i].myIdx == idx)
                        {
                            v_user[i].transform.position = new Vector3(float.Parse(txt[2]), float.Parse(txt[3]), 0f);
                            v_user[i].myMove = (MOVE_CONTROL)int.Parse(txt[4]);
                            break;
                        }
                }
            }
            else if (txt[0].Equals("LOGOUT"))
            {
                int idx = int.Parse(txt[1]);

                for (int i = 0; i < v_user.Count; i++)
                {
                    if (v_user[i] != null)
                        if (v_user[i].myIdx == idx)
                        {
                            Destroy(v_user[i].gameObject);
                            v_user.RemoveAt(i);
                            break;
                        }
                }
            }
            else if (txt[0].Equals("START"))
            {
                SGameMng.I.sTimer = "START";
                SGameMng.I.MapCtrl(int.Parse(txt[1]));
                _sound.gameBGM();
                SGameMng.I.InfoGame.SetActive(false);
                for (int i = 0; i < v_user.Count; i++)
                {
                    v_user[i].transform.localPosition = Vector3.zero;
                }
            }
            else if (txt[0].Equals("CHANGE"))
            {
                int idx = int.Parse(txt[1]);
                for (int i = 0; i < v_user.Count; i++)
                {
                    if (v_user[i] != null)
                        if (v_user[i].myIdx == idx)
                        {
                            v_user[i].color = (COLOR)int.Parse(txt[2]);
                            v_user[i].ChangeColor();
                            break;
                        }
                }
            }
            else if (txt[0].Equals("PROPER"))
            {

                int idx = int.Parse(txt[1]);
                for (int i = 0; i < v_user.Count; i++)
                {

                    if (v_user[i] != null)
                        if (v_user[i].myIdx == idx)
                        {
                            v_user[i].proper = (PROPER)int.Parse(txt[2]);
                            if (v_user[i].proper == PROPER.POLICE) { v_user[i].gameObject.tag = "Pcolider"; v_user[i].fSpeed = 10f; }
                            v_user[i].color = (COLOR)int.Parse(txt[3]);
                            Debug.Log((PROPER)int.Parse(txt[2]));
                            if (v_user[i].proper.Equals(PROPER.POLICE))
                            {
                                SGameMng.I.policeCount++;
                                SGameMng.I.policeCountTxt.text = SGameMng.I.policeCount + "";
                            }
                            else
                            {
                                SGameMng.I.thiefCount++;
                                SGameMng.I.thiefCountTxt.text = SGameMng.I.thiefCount + "";
                            }
                            v_user[i].SetUp();
                        }
                }
            }
            else if (txt[0].Equals("ATTACK"))
            {
                Debug.Log("ATTACK MESSAGE");
                for (int i = 0; i < v_user.Count; i++)
                {
                    if (v_user[i] != null)
                        if (v_user[i].myIdx == int.Parse(txt[1]))
                        {
                            // 공격
                            v_user[i].Attack();
                            break;
                        }
                }
            }
            else if (txt[0].Equals("TIME"))
            {
                SGameMng.I.sTimer = string.Format("{0,00}:{1,00}", int.Parse(txt[1]) / 60, int.Parse(txt[1]) % 60);
            }
            else if (txt[0].Equals("DIE"))
            {
                int idx = int.Parse(txt[1]);
                for (int i = 0; i < v_user.Count; i++)
                {
                    if (v_user[i] != null)
                        if (v_user[i].myIdx == idx)
                        {
                            v_user[i].isLive = false;
                            v_user[i].gameObject.SetActive(false);
                            if (v_user[i].proper.Equals(PROPER.POLICE))
                            {
                                SGameMng.I.policeCount--;
                                SGameMng.I.policeCountTxt.text = SGameMng.I.policeCount + "";
                            }
                            else
                            {
                                SGameMng.I.thiefCount--;
                                SGameMng.I.thiefCountTxt.text = SGameMng.I.thiefCount + "";
                            }
                            break;
                        }
                }
            }
            else if (txt[0].Equals("DONE"))
            {
                Debug.Log("G DONE");
                SGameMng.I.OpenResult((PROPER)int.Parse(txt[1]), int.Parse(txt[2]));
                SGameMng.I.sTimer = "READY";
                //// 게임  끝남
                //for (int i = 0; i < v_user.Count; i++)
                //{
                //    if (v_user[i] != null)
                //    {
                //        v_user[i].gameObject.SetActive(true);
                //        v_user[i].transform.position = Vector2.zero;
                //        // 관전 상태 해제
                //    }
                //}
            }
            else if (txt[0].Equals("WAIT"))
            {
                for (int i = 0; i < v_user.Count; i++)
                {
                    if (v_user[i].myIdx == v_user.Count - 1)
                        SceneManager.LoadScene("Login");
                }
                //SGameMng.I.MapCtrl(int.Parse(txt[1]));
                //_sound.gameBGM();
                //v_user[v_user.Count - 1].gameObject.SetActive(false);
                //v_user[v_user.Count - 1].isLive = false;
                Debug.Log("Wait");
            }
        }
        /**
         * @brief 기기에서 접속을 끊었을때 
         */
        void OnDestroy()
        {
            if (socket != null && socket.Connected)
            {
                SendMsg("DISCONNECT");
                Thread.Sleep(500);
                socket.Close();
            }
            StopCoroutine("PacketProc");
        }

        /**
         * @brief 게임내 로그아웃, 접속 종료
         */
        public void LogOutBT()
        {
            nowLoadingWindow.SetActive(true);
            OnDestroy();
            SceneManager.LoadScene("Intro");
        }

        /**
         * @brief 유저 이름 변경
         */
        public void setIpAddress(InputField address)
        {
            this.address = address.text;
        }

        /**
         * @brief 유저 이름 변경
         */
        public void setNickName(InputField nickName)
        {
            this.nickName = nickName.text;
        }

        /**
         * @brief 인터넷 연결되어 있는지 확인
         */
        public bool checkNetwork()
        {
            string HtmlText = GetHtmlFromUri("http://google.com");
            if (HtmlText.Equals(""))
            {
                // 연결 실패
                Debug.Log("인터넷 연결 실패");
            }
            else if (!HtmlText.Contains("schema.org/WebPage"))
            {
                // 비정상적인 루트일때
                Debug.Log("인터넷 연결 실패");
            }
            else
            {
                // 성공적인 연결
                Debug.Log("인터넷 연결 되있음");
                return true;
            }

            return false;
        }

        /**
         * @brief html 받아오기
         * @param resource url
         */
        public string GetHtmlFromUri(string resource)
        {
            string html = string.Empty;
            HttpWebRequest req = (HttpWebRequest)WebRequest.Create(resource);
            try
            {
                using (HttpWebResponse resp = (HttpWebResponse)req.GetResponse())
                {
                    bool isSuccess = (int)resp.StatusCode < 299 && (int)resp.StatusCode >= 200;
                    if (isSuccess)
                    {
                        using (StreamReader reader = new StreamReader(resp.GetResponseStream()))
                        {
                            char[] cs = new char[80];
                            reader.Read(cs, 0, cs.Length);
                            foreach (char ch in cs)
                            {
                                html += ch;
                            }
                        }
                    }
                }
            }
            catch
            {
                return "";
            }
            return html;
        }

        /**
         * @brief int 를 2바이트 데이터로 변환
         * @param val 변경할 변수
         */
        public static byte[] ShortToByte(int val)
        {
            byte[] temp = new byte[2];
            temp[1] = (byte)((val & 0x0000ff00) >> 8);
            temp[0] = (byte)((val & 0x000000ff));
            return temp;
        }
    }
}