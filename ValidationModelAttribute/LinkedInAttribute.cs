using System;
using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace CandidateHub.ValidationModelAttribute
{
    public class LinkedInAttribute : ValidationAttribute
    {
        private static readonly Regex LinkedInRegex = new(@"^(https?://)?(www\.)?linkedin\.com/.+", RegexOptions.IgnoreCase);

        public override bool IsValid(object value) =>
            string.IsNullOrEmpty(value as string) || Uri.IsWellFormedUriString(value as string, UriKind.Absolute) && LinkedInRegex.IsMatch(value as string);
    }
}
