using JetBrains.Annotations;
using StayInTarkov.Coop.Components.CoopGameComponents;
using StayInTarkov.Coop.Players;
using StayInTarkov.Coop.SITGameModes;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace StayInTarkov.Coop.FreeCamera
{
    /// <summary>
    /// A simple free camera to be added to a Unity game object.
    /// 
    /// Full credit to Ashley Davis on GitHub for the inital code:
    /// https://gist.github.com/ashleydavis/f025c03a9221bc840a2b
    /// 
    /// This is HEAVILY based on Terkoiz's work found here. Thanks for your work Terkoiz! 
    /// https://dev.sp-tarkov.com/Terkoiz/Freecam/raw/branch/master/project/Terkoiz.Freecam/FreecamController.cs
    /// </summary>
    public class FreeCamera : MonoBehaviour
    {
        public bool IsActive = false;
        private EFT.Player CurrentPlayer;
        private bool isFollowing = false;

        [UsedImplicitly]
        public void Update()
        {
            if (!IsActive)
            {
                return;
            }

            bool keyDown = Input.GetKeyDown(KeyCode.Mouse0);
            if (keyDown)
            {
                SITGameComponent sitGameComponent = SITGameComponent.GetCoopGameComponent();
                bool flag2 = sitGameComponent == null;
                if (flag2)
                {
                    return;
                }
                List<CoopPlayer> list = new List<CoopPlayer>();
                list.AddRange(sitGameComponent.Players.Values.Where((CoopPlayer x) => !x.IsYourPlayer && sitGameComponent.ProfileIdsUser.Contains(x.ProfileId) && x.HealthController.IsAlive));
                List<CoopPlayer> list2 = list;
                bool flag3 = list2.Count > 0;
                if (flag3)
                {
                    bool key = Input.GetKey(KeyCode.Space);
                    using (List<CoopPlayer>.Enumerator enumerator = list2.GetEnumerator())
                    {
                        if (enumerator.MoveNext())
                        {
                            CoopPlayer coopPlayer = enumerator.Current;
                            bool flag4 = this.CurrentPlayer == null;
                            if (flag4)
                            {
                                this.CurrentPlayer = list2[0];
                                bool flag5 = key;
                                if (flag5)
                                {
                                    this.AttachToPlayer();
                                }
                                else
                                {
                                    this.JumpToPlayer();
                                }
                            }
                            else
                            {
                                int num = list2.IndexOf(this.CurrentPlayer) + 1;
                                bool flag6 = list2.Count - 1 >= num;
                                if (flag6)
                                {
                                    this.CurrentPlayer = list2[num];
                                    bool flag7 = key;
                                    if (flag7)
                                    {
                                        this.AttachToPlayer();
                                    }
                                    else
                                    {
                                        this.JumpToPlayer();
                                    }
                                }
                                else
                                {
                                    this.CurrentPlayer = list2[0];
                                    bool flag8 = key;
                                    if (flag8)
                                    {
                                        this.AttachToPlayer();
                                    }
                                    else
                                    {
                                        this.JumpToPlayer();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    bool flag9 = this.isFollowing;
                    if (flag9)
                    {
                        this.isFollowing = false;
                        base.transform.parent = null;
                    }
                }
            }
            bool keyDown2 = Input.GetKeyDown(KeyCode.Mouse1);
            if (keyDown2)
            {
                SITGameComponent sitGameComponent = SITGameComponent.GetCoopGameComponent();
                bool flag10 = sitGameComponent == null;
                if (flag10)
                {
                    return;
                }
                List<CoopPlayer> list3 = new List<CoopPlayer>();
                list3.AddRange(sitGameComponent.Players.Values.Where((CoopPlayer x) => !x.IsYourPlayer && sitGameComponent.ProfileIdsUser.Contains(x.ProfileId) && x.HealthController.IsAlive));
                List<CoopPlayer> list4 = list3;
                bool flag11 = list4.Count > 0;
                if (flag11)
                {
                    bool key2 = Input.GetKey(KeyCode.Space);
                    using (List<CoopPlayer>.Enumerator enumerator2 = list4.GetEnumerator())
                    {
                        if (enumerator2.MoveNext())
                        {
                            CoopPlayer coopPlayer2 = enumerator2.Current;
                            bool flag12 = this.CurrentPlayer == null;
                            if (flag12)
                            {
                                this.CurrentPlayer = list4[0];
                                bool flag13 = key2;
                                if (flag13)
                                {
                                    this.AttachToPlayer();
                                }
                                else
                                {
                                    this.JumpToPlayer();
                                }
                            }
                            else
                            {
                                int num2 = list4.IndexOf(this.CurrentPlayer) - 1;
                                bool flag14 = num2 >= 0;
                                if (flag14)
                                {
                                    this.CurrentPlayer = list4[num2];
                                    bool flag15 = key2;
                                    if (flag15)
                                    {
                                        this.AttachToPlayer();
                                    }
                                    else
                                    {
                                        this.JumpToPlayer();
                                    }
                                }
                                else
                                {
                                    this.CurrentPlayer = list4[list4.Count - 1];
                                    bool flag16 = key2;
                                    if (flag16)
                                    {
                                        this.AttachToPlayer();
                                    }
                                    else
                                    {
                                        this.JumpToPlayer();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    bool flag17 = this.isFollowing;
                    if (flag17)
                    {
                        this.isFollowing = false;
                        base.transform.parent = null;
                    }
                }
            }



            bool flag18 = this.isFollowing;
            if (!flag18)
            {

                var fastMode = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);
                var movementSpeed = fastMode ? 10f : 2f;

                if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
                {
                    transform.position += (-transform.right * (movementSpeed * Time.deltaTime));
                }

                if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
                {
                    transform.position += (transform.right * (movementSpeed * Time.deltaTime));
                }

                if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
                {
                    transform.position += (transform.forward * (movementSpeed * Time.deltaTime));
                }

                if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
                {
                    transform.position += (-transform.forward * (movementSpeed * Time.deltaTime));
                }

                if (true)
                {
                    if (Input.GetKey(KeyCode.Q))
                    {
                        transform.position += (transform.up * (movementSpeed * Time.deltaTime));
                    }

                    if (Input.GetKey(KeyCode.E))
                    {
                        transform.position += (-transform.up * (movementSpeed * Time.deltaTime));
                    }

                    if (Input.GetKey(KeyCode.R) || Input.GetKey(KeyCode.PageUp))
                    {
                        transform.position += (Vector3.up * (movementSpeed * Time.deltaTime));
                    }

                    if (Input.GetKey(KeyCode.F) || Input.GetKey(KeyCode.PageDown))
                    {
                        transform.position += (-Vector3.up * (movementSpeed * Time.deltaTime));
                    }
                }

                float newRotationX = transform.localEulerAngles.y + Input.GetAxis("Mouse X") * 3f;
                float newRotationY = transform.localEulerAngles.x - Input.GetAxis("Mouse Y") * 3f;
                transform.localEulerAngles = new Vector3(newRotationY, newRotationX, 0f);

                //if (FreecamPlugin.CameraMousewheelZoom.Value)
                //{
                //    float axis = Input.GetAxis("Mouse ScrollWheel");
                //    if (axis != 0)
                //    {
                //        var zoomSensitivity = fastMode ? FreecamPlugin.CameraFastZoomSpeed.Value : FreecamPlugin.CameraZoomSpeed.Value;
                //        transform.position += transform.forward * (axis * zoomSensitivity);
                //    }
                //}
            }
        }

        public void JumpToPlayer()
        {
            base.transform.position = new Vector3(this.CurrentPlayer.Transform.position.x - 2f, this.CurrentPlayer.Transform.position.y + 2f, this.CurrentPlayer.Transform.position.z);
            base.transform.LookAt(new Vector3(this.CurrentPlayer.Transform.position.x, this.CurrentPlayer.Transform.position.y + 1f, this.CurrentPlayer.Transform.position.z));
            bool flag = this.isFollowing;
            if (flag)
            {
                this.isFollowing = false;
                base.transform.parent = null;
            }
        }
        public void AttachToPlayer()
        {
            base.transform.parent = this.CurrentPlayer.PlayerBones.Head.Original;
            base.transform.localPosition = new Vector3(-0.1f, -0.07f, -0.17f);
            base.transform.localEulerAngles = new Vector3(260f, 80f, 0f);
            this.isFollowing = true;
        }

        public void SetActive(bool status)
        {
            this.IsActive = status;
            this.isFollowing = false;
            base.transform.parent = null;
        }

        [UsedImplicitly]
        private void OnDestroy()
        {
            Destroy(this);
        }
    }
}