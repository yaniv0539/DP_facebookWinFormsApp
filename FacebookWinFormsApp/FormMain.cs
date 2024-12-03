﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using FacebookWrapper.ObjectModel;
using FacebookWrapper;

namespace BasicFacebookFeatures
{
    public partial class FormMain : Form
    {
        public FormMain()
        {
            InitializeComponent();
            FacebookWrapper.FacebookService.s_CollectionLimit = 25;
        }

        FacebookWrapper.LoginResult m_LoginResult;//??????????
        User m_LoggedInUser;//???????????????

        private void buttonLogin_Click(object sender, EventArgs e)
        {
            Clipboard.SetText("design.patterns");

            if (m_LoginResult == null)
            {
                login();
            }
        }

        private void login()
        {
            m_LoginResult = FacebookService.Login(
                /// (This is Desig Patter's App ID. replace it with your own)
                textBoxAppID.Text,
                /// requested permissions:
                "email",
                "public_profile",
                /// add any relevant permissions
                "user_age_range",
                "user_birthday",
                "user_events",
                "user_friends",
                "user_gender",
                "user_hometown",
                "user_likes",
                "user_link",
                "user_location",
                "user_photos",
                "user_posts",
                "user_videos"
                );

            if (!string.IsNullOrEmpty(m_LoginResult.AccessToken))
            {
                buttonLogin.Text = $"Logged in as {m_LoginResult.LoggedInUser.Name}";
                buttonLogin.BackColor = Color.LightGreen;
                pictureBoxProfile.ImageLocation = m_LoginResult.LoggedInUser.PictureNormalURL;
                buttonLogin.Enabled = false;
                buttonLogout.Enabled = true;
                m_LoggedInUser = m_LoginResult.LoggedInUser;
                fetchUserFriends();// לסדר
            }
            else
            {
                MessageBox.Show(m_LoginResult.ErrorMessage, "Login Failed");
                m_LoginResult = null;
            }
        }

        private void buttonLogout_Click(object sender, EventArgs e)
        {
            FacebookService.LogoutWithUI();
            buttonLogin.Text = "Login";
            buttonLogin.BackColor = buttonLogout.BackColor;
            m_LoginResult = null;
            buttonLogin.Enabled = true;
            buttonLogout.Enabled = false;
        }

        private void fetchUserFriends()
        {
            searchableListWithTitleFriends.Items.Clear();
            searchableListWithTitleFriends.DisplayMember = "Name";
            try
            {
                foreach (User friend in m_LoggedInUser.Friends)
                {
                    searchableListWithTitleFriends.Items.Add(friend);
                }

                if (searchableListWithTitleFriends.Items.Count == 0)
                {
                    //comboBoxFriendsSort.Enabled = false;
                    searchableListWithTitleFriends.Items.Add("Found no friends :(");
                }
            }
            catch (Exception exception)
            {
                //comboBoxFriendsSort.Enabled = false;
                MessageBox.Show("failed to fetch friends");
            }
        }

        private void searchableListWithTitleFriends_SelectedIndexChanged(object sender, EventArgs e)
        {
            User selectedFriend = searchableListWithTitleFriends.SelectedItem as User;
            pictureBoxchoosenFriend.LoadAsync(selectedFriend.PictureNormalURL);
        }

        
    }
}
