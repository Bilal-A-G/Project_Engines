using System.Runtime.InteropServices;
using UnityEngine;

namespace DLLs
{
    //Thank you GPT
    [StructLayout(LayoutKind.Sequential)]
    public struct GameData
    {
        public int score;
        public float health;
        public int level;
    }

    public static class StupidSaving
    {
        private static readonly string FileLoc = Application.dataPath + "/SaveData.dat";

        [DllImport("SaveSystem")] 
        private static extern void SaveGameData(GameData data, string path);

        [DllImport("SaveSystem")]
        private static extern void LoadGameData(out GameData data, string path);

        public static void SaveGame(GameData d)
        {
            SaveGameData(d, FileLoc);
        }

        public static GameData LoadGame()
        {
            LoadGameData(out GameData d, FileLoc);
            return d;
        }



    }
}