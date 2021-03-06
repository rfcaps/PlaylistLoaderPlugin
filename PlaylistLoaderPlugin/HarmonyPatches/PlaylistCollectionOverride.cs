﻿using System;
using System.Collections.Generic;
using HarmonyLib;

namespace PlaylistLoaderLite.HarmonyPatches
{
    [HarmonyPatch(typeof(AnnotatedBeatmapLevelCollectionsViewController), "SetData",
        new Type[] {
        typeof(IReadOnlyList<IAnnotatedBeatmapLevelCollection>), typeof(int), typeof(bool)})]
    public class PlaylistCollectionOverride
    {
        private static IAnnotatedBeatmapLevelCollection[] loadedPlaylists;

        internal static void Prefix(ref IAnnotatedBeatmapLevelCollection[] annotatedBeatmapLevelCollections)
        {
            // Check if annotatedBeatmapLevelCollections is empty (Versus Tab)
            if (annotatedBeatmapLevelCollections.Length == 0)
                return;
            // Checks if this is the playlists view
            if (annotatedBeatmapLevelCollections[0] is CustomBeatmapLevelPack)
            {
                IAnnotatedBeatmapLevelCollection[] allCustomBeatmapLevelCollections = new IAnnotatedBeatmapLevelCollection[loadedPlaylists.Length + annotatedBeatmapLevelCollections.Length];

                for (int i = 0; i < annotatedBeatmapLevelCollections.Length; i++)
                    allCustomBeatmapLevelCollections[i] = annotatedBeatmapLevelCollections[i];

                int j = 0;
                for (int i = annotatedBeatmapLevelCollections.Length; i < allCustomBeatmapLevelCollections.Length; i++)
                    allCustomBeatmapLevelCollections[i] = loadedPlaylists[j++];

                annotatedBeatmapLevelCollections = allCustomBeatmapLevelCollections;
            }
        }

        public static int RefreshPlaylists()
        {
            loadedPlaylists = LoadPlaylistScript.Load();
            return loadedPlaylists.Length;
        }
    }
}
