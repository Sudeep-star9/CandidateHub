using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CandidateHub.ValidationModelAttribute
{
    public class GitHubUrlAttribute : ValidationAttribute
    {
        private static readonly Regex GitHubRegex = new(@"^(https?://)?(www\.)?github\.com/[a-zA-Z0-9-]+(/[a-zA-Z0-9-]+)?$", RegexOptions.IgnoreCase);

        public override bool IsValid(object value) =>
            string.IsNullOrEmpty(value as string) || Uri.IsWellFormedUriString(value as string, UriKind.Absolute) && GitHubRegex.IsMatch(value as string);
    }
}
