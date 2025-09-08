using System;
using Newtonsoft.Json;
using SelfishFramework.Src.Core.Components;
using SelfishFramework.Src.Core.Features.Serialization.Attributes;

namespace SelfishFramework.Src.Core.Features.Serialization
{
    public static partial class SelfishSerialization
    {
        // public static string JsonSerialize<T>() where T : struct, IComponent
        // {
        //     
        // }
        //
        // public static void JsonDeserialize<T>(string json)
    }

    public interface IJsonResolver<T, TResolver> where T : struct, IComponent
    {
        public TResolver Write(ref T component);
        public void Read(ref T component);
    }
    
    // [Serializable]
    // [SelfishSerialize]
    // public struct PlayerProgressComponent : IComponent
    // {
    //     [SField]
    //     public int LevelIndex;
    //     [SField]
    //     public bool TutorialPassed;
    // }
    //
    // [JsonObject]
    // public struct PlayerProgressComponentJsonResolver : IJsonResolver<PlayerProgressComponent, PlayerProgressComponentJsonResolver>
    // {
    //     [JsonProperty("LevelIndex")]
    //     public int LevelIndex;
    //     [JsonProperty("TutorialPassed")]
    //     public bool TutorialPassed;
    //     
    //     public PlayerProgressComponentJsonResolver Write(ref PlayerProgressComponent component)
    //     {
    //         LevelIndex = component.LevelIndex;
    //         TutorialPassed = component.TutorialPassed;
    //         return this;
    //     }
    //
    //     public void Read(ref PlayerProgressComponent playerProgressComponent)
    //     {
    //         playerProgressComponent.LevelIndex = LevelIndex;
    //         playerProgressComponent.TutorialPassed = TutorialPassed;
    //     }
    // }
}