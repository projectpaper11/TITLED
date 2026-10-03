using UnityEngine;

namespace GameSystem
{
    public class LeaveButton : MonoBehaviour
    {
        public void OnLeaveButtonClick()
        {
            Application.Quit();
        }

        public void OnUntitledButtonClick()
        {
            Application.OpenURL("https://drive.google.com/file/d/1AcWv_eF_XfA8R3Jd09QqlqISCb0YALiQ/view?pli=1");
        }
    }
}