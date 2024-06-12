using Steamworks;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class FriendInfo : MonoBehaviour
{
    [SerializeField] private TMP_Text friendName;
    [SerializeField] private Button inviteBtn;
    public bool IsOnline
    {
        set
        {
            inviteBtn.enabled = value;
            Color newColor = value ? new Vector4(1, 1, 1, 1) : new Vector4(1, 1, 1, .2f);
            friendName.color = newColor;
            inviteBtn.image.color = newColor;
        }
    }
    public Friend friend { get; private set; } 

    public void Init(Friend _friend)
    {
        friend = _friend;
        friendName.text = friend.Name;
        inviteBtn.onClick.AddListener(() =>
        {
            SteamManager.Instance.currentLobby?.InviteFriend(friend.Id);
            gameObject.SetActive(false);
            Invoke("ForceEnable",2f);
        });
        IsOnline = false;
    }


    private void ForceEnable()
    {
        gameObject.SetActive(true);
    }
}
