using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using _Source.Code._AKFramework.AKCore.Editor;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._Core.Installers;
using Leopotam.EcsLite;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Editor
{
    public class AKEntitiesBrowserWindow : EditorWindow, IEcsWorldEventListener , IAKEditorTool
    {
        private IAKWorldService _worldsService;

        private EcsWorld _world;
        
        private HashSet<int> _entities;

        public static AKEntityDataWindow DataWindow;
        private Vector2 _entityScrollPos;
        private string _searchString = "";

        [MenuItem("Window/ECS/Entity Browser Window")]
        public static void OpenEntityBrowserWindow() 
        {
            var window = GetWindow<AKEntitiesBrowserWindow>(nameof(AKEntitiesBrowserWindow));
            window.titleContent = new GUIContent
            {
                text = "Entity Browser Window"
            };
            window.Show();
        }

        public AKEntitiesBrowserWindow()
        {
            if(AKContextRoot.Container == null) return;
            _worldsService = AKContextRoot.Container.Resolve<IAKWorldService>();
            _world = _worldsService.Default;
            _entities = new HashSet<int>();
            _world.AddEventListener (this);
            var entities = Array.Empty<int> ();
            var entitiesCount = _world.GetAllEntities (ref entities);
            for (var i = 0; i < entitiesCount; i++) 
            {
                OnEntityCreated (entities[i]);
            }
            
            _searchString = "";
        }

        public void OnEnable()
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
            EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
        }
        
        private void OnDisable() 
        {
            EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        }

        public void OnEntityCreated(int entity)
        {
            if(_entities.Contains(entity)) return;

            _entities.Add(entity);
        }

        public void OnEntityChanged(int entity, short poolId, bool added)
        {
            
        }

        public void OnEntityDestroyed(int entity)
        {
            if (_entities.Contains(entity))
            {
                _entities.Remove(entity);
            }
        }

        public void OnFilterCreated(EcsFilter filter)
        {
            
        }

        public void OnWorldResized(int newSize)
        {
            
        }

        public void OnWorldDestroyed(EcsWorld world)
        {
            _world.RemoveEventListener (this);
        }

        public void OnGUI()
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.FindStyle("Toolbar"));
            EditorGUILayout.Space();
            _searchString = GUILayout.TextField(_searchString, GUI.skin.FindStyle("ToolbarSeachTextField"));
            if (GUILayout.Button("", GUI.skin.FindStyle("ToolbarSeachCancelButton")))
            {
                _searchString = "";
                GUI.FocusControl(null);
            }
            EditorGUILayout.EndHorizontal();
            
            if(_worldsService == null) return;
            
            var widthWindow = 0f;
            _entityScrollPos = EditorGUILayout.BeginScrollView(_entityScrollPos);

            EditorGUILayout.BeginHorizontal();
            
            foreach (var value in _entities)
            {
                var entityName = $"{value}";
                if (_world.GetPool<GameObjectRef>().Has(value))
                {
                    entityName += $"_{_world.GetPool<GameObjectRef>().Get(value).instance.name}";
                }
                
                if(entityName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) < 0 && !IsFieldContains(value, _searchString)) continue;
                

                widthWindow += entityName.Length * 10f;
                
                if (widthWindow >= position.width)
                {
                    widthWindow = 0f;
                    EditorGUILayout.EndHorizontal();
                    EditorGUILayout.BeginHorizontal();
                }
                
                if (GUILayout.Button(entityName, GUI.skin.button, GUILayout.Width(entityName.Length * 10f), GUILayout.Height(50f)))
                {
                    if(DataWindow == null)
                        DataWindow = GetWindow<AKEntityDataWindow>($"{nameof(AKEntityDataWindow)}_{value}");

                    DataWindow.Entity = value;
                    DataWindow.titleContent.text = $"{nameof(AKEntityDataWindow)}_{value}";
                    if (_world.GetPool<GameObjectRef>().Has(value))
                    {
                        Selection.activeGameObject = _world.GetPool<GameObjectRef>().Get(value).instance;
                        SceneView.lastActiveSceneView.FrameSelected();
                    }
                }
            }
            
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.EndScrollView();
        }

        private async void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.EnteredPlayMode)
            {
                await Task.FromResult(AKContextRoot.Container != null);
                _worldsService = AKContextRoot.Container.Resolve<IAKWorldService>();
                _world = _worldsService.Default;
                _entities = new HashSet<int>();
                _world.AddEventListener(this);
                var entities = Array.Empty<int>();
                var entitiesCount = _world.GetAllEntities(ref entities);
                for (var i = 0; i < entitiesCount; i++)
                {
                    OnEntityCreated(entities[i]);
                }
            }
        }

        private bool IsFieldContains(int entity, string searchString)
        {
            if (searchString == string.Empty) return true;
            
            object[] list = { };
            var count = _world.GetComponents(entity, ref list);

            for (int i = 0; i < count; i++)
            {
                var fieldInfo = list[i].GetType().GetFields();
                foreach (var field in fieldInfo)
                {
                    if (field.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) return true;
                }
            }

            return false;
        }

        public string Title => "Entity Browser";
    }

    internal class UnitTask
    {
    }
}