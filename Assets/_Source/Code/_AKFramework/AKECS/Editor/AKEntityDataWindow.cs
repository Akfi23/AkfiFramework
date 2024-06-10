using System;
using System.Collections.Generic;
using System.Reflection;
using _Source.Code._AKFramework.AKECS.Runtime;
using _Source.Code._Core.Installers;
using Leopotam.EcsLite;
using Leopotam.EcsLite.UnityEditor;
using UnityEditor;
using UnityEngine;

namespace _Source.Code._AKFramework.AKECS.Editor
{
    public class AKEntityDataWindow:EditorWindow, IEcsWorldEventListener
    {
        public int Entity;

        private EcsWorld _world;
        private static object[] _componentsCache = new object[32];
        const int MaxFieldToStringLength = 128;
        private string _searchString = "";
        private int _index;
        private Vector2 _scrollPosition;

        [MenuItem("Window/ECS/Entity Data Window")]
        public static void OpenEcsDebugWindow()
        {
            GetWindow<AKEntityDataWindow>(nameof(AKEntityDataWindow)).Show();
        }

        static AKEntityDataWindow()
        {
            AKEntity.OnPingEntity -= PingEntity;
            AKEntity.OnPingEntity += PingEntity;
        }
        

        public AKEntityDataWindow()
        {
            _world = AKContextRoot.Container.Resolve<IAKWorldService>().Default;
            AKEntity.OnPingEntity -= PingEntity;
            AKEntity.OnPingEntity += PingEntity;
        }

        public void OnEntityCreated(int entity)
        {
            
        }

        public void OnEntityChanged(int entity, short poolId, bool added)
        {
            
        }

        public void OnEntityDestroyed(int entity)
        {
            
        }

        public void OnFilterCreated(EcsFilter filter)
        {
            
        }

        public void OnWorldResized(int newSize)
        {
            
        }

        public void OnWorldDestroyed(EcsWorld world)
        {
            
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

        private void OnPlayModeStateChanged(PlayModeStateChange state)
        {
            if (state == PlayModeStateChange.ExitingPlayMode)
            {
                while (HasOpenInstances<AKEntityDataWindow>())
                {
                    GetWindow<AKEntityDataWindow>().Close();
                }
            }
        }

        private void Update()
        {
            Repaint();
        }

        private void OnGUI()
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
            DrawData();
            DrawComponents();
        }

        private void DrawData()
        {
            EditorGUILayout.BeginHorizontal(GUI.skin.box);
            EditorGUILayout.BeginVertical(GUI.skin.box);
            EditorGUILayout.LabelField($"Entity Id: {Entity}");
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
        }

        private void DrawComponents()
        {
            var count = _world.GetComponents(Entity, ref _componentsCache);

            _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition);

            for (var i = 0; i < count; i++)
            {
                var component = _componentsCache[i];
                _componentsCache[i] = null;
                var type = component.GetType();
                var typeName = EditorExtensions.GetCleanGenericTypeName(type);
                
                if (_searchString != string.Empty && typeName.IndexOf(_searchString, StringComparison.OrdinalIgnoreCase) < 0 && !IsFieldContains(type, _searchString)) continue;

                EditorGUILayout.BeginHorizontal(GUI.skin.box);
                EditorGUILayout.BeginVertical(GUI.skin.box);
                var pool = _world.GetPoolByType(type);
                var (rendered, changed, newValue) =
                    EcsComponentInspectors.Render(typeName, type, component, _world);
                if (!rendered)
                {
                    EditorGUILayout.LabelField(typeName, EditorStyles.boldLabel);
                    var indent = EditorGUI.indentLevel;
                    EditorGUI.indentLevel++;
                    foreach (var field in type.GetFields(BindingFlags.Instance | BindingFlags.Public |
                                                         BindingFlags.NonPublic))
                    {
                        DrawTypeField(component, pool, field);
                    }

                    EditorGUI.indentLevel = indent;
                }
                else
                {
                    if (changed)
                    {
                        pool.SetRaw(Entity, newValue);
                    }
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.BeginVertical(GUI.skin.box);
                
                EditorGUILayout.LabelField("Actions");

                if (GUILayout.Button("Open IDE", GUI.skin.button))
                {
                    var assets = AssetDatabase.FindAssets($"t: Script {typeName}");
                    if (assets.Length > 0)
                    {
                        for (int a = 0; a < assets.Length; a++)
                        {
                            if (AssetDatabase.GUIDToAssetPath(assets[a]).Contains($"/{typeName}.cs"))
                            {
                                if (EditorUtility.DisplayDialog($"Open IDE", $"Open {typeName}?", "Yes",
                                        "Cancel"))
                                {
                                    AssetDatabase.OpenAsset(AssetDatabase.LoadMainAssetAtPath(AssetDatabase.GUIDToAssetPath(assets[a])));
                                }
                                break;
                            }
                        }
                    }
                }

                if (GUILayout.Button("Remove", GUI.skin.button))
                {
                    pool.Del(Entity);
                }

                EditorGUILayout.EndVertical();

                EditorGUILayout.EndHorizontal();
                EditorGUILayout.Space();
            }

            EditorGUILayout.EndScrollView();
        }

        private void DrawTypeField(object component, IEcsPool pool, FieldInfo field)
        {
            var fieldValue = field.GetValue(component);
            var fieldType = field.FieldType;
            var (rendered, changed, newValue) =
                EcsComponentInspectors.Render(field.Name, fieldType, fieldValue, _world);
            
            if (!rendered)
            {
                if (fieldType == typeof(UnityEngine.Object) || fieldType.IsSubclassOf(typeof(UnityEngine.Object)))
                {
                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(field.Name, GUILayout.MaxWidth(EditorGUIUtility.labelWidth - 16));
                    var newObjValue = EditorGUILayout.ObjectField((UnityEngine.Object)fieldValue, fieldType, true);
                    if (newObjValue != (UnityEngine.Object)fieldValue)
                    {
                        field.SetValue(component, newObjValue);
                        pool.SetRaw(Entity, component);
                    }

                    GUILayout.EndHorizontal();
                    return;
                }

                if (fieldType.IsEnum)
                {
                    var isFlags = Attribute.IsDefined(fieldType, typeof(FlagsAttribute));
                    var (enumChanged, enumNewValue) =
                        EcsComponentInspectors.RenderEnum(field.Name, fieldValue, isFlags);
                    if (enumChanged)
                    {
                        field.SetValue(component, enumNewValue);
                        pool.SetRaw(Entity, component);
                    }

                    return;
                }

                var strVal = fieldValue != null
                    ? string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}", fieldValue)
                    : "null";
                if (strVal.Length > MaxFieldToStringLength)
                {
                    strVal = strVal.Substring(0, MaxFieldToStringLength);
                }

                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(field.Name);
                EditorGUILayout.SelectableLabel(strVal, GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
                GUILayout.EndHorizontal();
            }
            else
            {
                if (changed)
                {
                    // update value.
                    field.SetValue(component, newValue);
                    pool.SetRaw(Entity, component);
                }
            }
        }
        
        private bool IsFieldContains(Type type, string searchString)
        {
            if (searchString == string.Empty) return false;
            
            var fieldInfo = type.GetFields();
            foreach (var field in fieldInfo)
            {
                if (field.Name.IndexOf(searchString, StringComparison.OrdinalIgnoreCase) >= 0) return true;
                
            }
            return false;
        }

        private static AKEntityDataWindow _window;
        
        private static void PingEntity(int entity)
        {
            var name = $"{nameof(AKEntityDataWindow)}_{entity}";
            if (_window != null)
            {
                if (_window.name == name)
                {
                    _window.Focus();
                    return;
                }
                _window.Close();
                _window = null;
            }
            _window = CreateWindow<AKEntityDataWindow>(name);
            _window.Entity = entity;
            _window.ShowPopup();
        }
    }

    internal static class EcsComponentInspectors
    {
        static readonly Dictionary<Type, IEcsComponentInspector> Inspectors = new();

        static EcsComponentInspectors()
        {
            foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
            {
                foreach (var type in assembly.GetTypes())
                {
                    if (typeof(IEcsComponentInspector).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
                    {
                        if (Activator.CreateInstance(type) is IEcsComponentInspector inspector)
                        {
                            var componentType = inspector.GetFieldType();
                            if (!Inspectors.TryGetValue(componentType, out var prevInspector)
                                || inspector.GetPriority() > prevInspector.GetPriority())
                            {
                                Inspectors[componentType] = inspector;
                            }
                        }
                    }
                }
            }
        }

        public static (bool, bool, object) Render(string label, Type type, object value, EcsWorld world)
        {
            if (Inspectors.TryGetValue(type, out var inspector))
            {
                var (changed, newValue) = inspector.OnGui(label, value, world);
                return (true, changed, newValue);
            }

            return (false, false, null);
        }

        public static (bool, object) RenderEnum(string label, object value, bool isFlags)
        {
            var enumValue = (Enum)value;
            Enum newValue;
            if (isFlags)
            {
                newValue = EditorGUILayout.EnumFlagsField(label, enumValue);
            }
            else
            {
                newValue = EditorGUILayout.EnumPopup(label, enumValue);
            }

            if (Equals(newValue, value))
            {
                return (default, default);
            }

            return (true, newValue);
        }
    }

    public interface IEcsComponentInspector
    {
        Type GetFieldType();
        int GetPriority();
        (bool, object) OnGui(string label, object value, EcsWorld world);
    }

    public abstract class EcsComponentInspectorTyped<T> : IEcsComponentInspector
    {
        public Type GetFieldType() => typeof(T);
        public virtual bool IsNullAllowed() => false;
        public virtual int GetPriority() => 0;

        public (bool, object) OnGui(string label, object value, EcsWorld world)
        {
            if (value == null && !IsNullAllowed())
            {
                GUILayout.BeginHorizontal();
                EditorGUILayout.PrefixLabel(label);
                EditorGUILayout.SelectableLabel("null", GUILayout.MaxHeight(EditorGUIUtility.singleLineHeight));
                GUILayout.EndHorizontal();
                return (default, default);
            }

            var typedValue = (T)value;
            var changed = OnGuiTyped(label, ref typedValue, world);
            if (changed)
            {
                return (true, typedValue);
            }

            return (default, default);
        }

        public abstract bool OnGuiTyped(string label, ref T value, EcsWorld world);
    }
}