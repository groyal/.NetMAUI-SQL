using System.Runtime.CompilerServices;

namespace BeebopNoteApp.Domain.Models;

public static class Constants
{
    public static class PriorityLevels
    {
        public const string Urgent = "URGENT";
        public const string Normal = "NORMAL";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static string Normalize(string priority)
        {
            return priority.Trim().ToUpper();
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsUrgent(string priority)
        {
            return Normalize(priority) == Urgent;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static bool IsNormal(string priority)
        {
            return Normalize(priority) == Normal;
        }
    }
}