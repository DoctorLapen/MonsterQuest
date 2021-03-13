using System;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using UnityEngine;
using Zenject;

namespace MonsterQuest
{
    public class LeaderboardPlayFabController : MonoBehaviour, ILeaderboardController
    {

        [Inject]
        private ILeaderboardView _leaderboardView;

        [SerializeField]
        private LeaderboardPlayFabSettings _leaderboardSettings;


        private void Start()
        {
            _leaderboardView.UpdateBoard += ShowLeaderboard;
        }

        private void OnEnable()
        {
            ShowLeaderboard();
        }

        public void SendScore(int score)
        {
            var request = new UpdatePlayerStatisticsRequest
            {
                Statistics = new List<StatisticUpdate>
                {
                    new StatisticUpdate
                    {
                        StatisticName = _leaderboardSettings.StatisticName,
                        Value = score
                    }
                }
            };
            PlayFabClientAPI.UpdatePlayerStatistics(request,OnUpdateSuccess,OnUpdateFailure);
        }

        public void ShowLeaderboard()
        {
            var request = new GetLeaderboardRequest
            {
                StatisticName = _leaderboardSettings.StatisticName,
                StartPosition = _leaderboardSettings.StartPosition,
                MaxResultsCount = _leaderboardSettings.MaxResultCount
            };
            PlayFabClientAPI.GetLeaderboard(request,OnGetLeaderboard,OnUpdateFailure);
        }

        private void OnGetLeaderboard(GetLeaderboardResult result)
        {
            _leaderboardView.Clear();
            for (int i = 0; i < result.Leaderboard.Count; i++)
            {
                PlayerLeaderboardEntry entry = result.Leaderboard[i];
                int position = entry.Position + 1;
                LeaderboardItem item = new LeaderboardItem(position, entry.DisplayName, entry.StatValue);
                _leaderboardView.AddItem(item);
            }
        }

        private void OnUpdateFailure(PlayFabError obj)
        {
            Debug.LogError(obj.GenerateErrorReport());
        }

        private void OnUpdateSuccess(UpdatePlayerStatisticsResult obj)
        {
            Debug.Log("UpdateSuccess");
        }
    }
}