using _Client_.Scripts.Interfaces;
using _Source.Code._AKFramework.AKCore.Runtime;
using _Source.Code.Interfaces;
using UnityEngine;

namespace _Source.Code.Services
{
    public class PlayerDataService : IAKService
    {
        [AKInject]
        private ISaveService _saveService;
        
        private Vector3 _playerPos = Vector3.zero;

        private Vector2 _screenPlayerPos;
        public Vector2 ScreenPlayerPos => _screenPlayerPos;
        
        public void SetPlayerPos(Vector3 pos)
        {
            _playerPos = pos;
        }

        public void SetScreenPlayerPos(Vector2 pos)
        {
            _screenPlayerPos = pos;
        }

        public Vector3 GetPlayerPos()
        {
            // _playerPos = _saveService.Load("PlayerPos", new OurVector3()).ToVector3(); // OurVector3 need to save data.
            return _playerPos;                                                           // Use Other JSON serializer or make custom data class convert structs
        } 

        public void SavePlayerPos()
        {
            if(_playerPos.Equals(Vector3.zero)) return;
            // _saveService.Save("PlayerPos", new OurVector3(_playerPos));
        }

        public bool HasPlayerPos()
        {
            return _saveService.Has("PlayerPos");
        }

        public void ClearPos()
        {
            _saveService.Remove("PlayerPos");
        }
    }
}
