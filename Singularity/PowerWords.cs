using System;
using System.Collections.Generic;

namespace Singularity
{
	/// <summary>
	/// Quick list of powerful and persuasive English words.
	/// </summary>
	public static class PowerWords
	{
		public static List<String> Persuasive => _persuasive ?? (_persuasive = new List<String>()
		{
			"You",
			"Free",
			"Because",
			"Instantly",
			"New"
		});
		private static List<String> _persuasive;

		public static List<String> Influential => _influential ?? (_influential = new List<String>()
		{
			"Suddenly", "Now", "Annoucing", "Introducing", "Improvement",
			"Amazing", "Sensational", "Remarkable", "Revolutionary", "Startling",
			"Miracle", "Magic", "Offer", "Quick", "Easy",
			"Wanted", "Challenge", "Compare", "Bargin", "Hurry"
		});
		private static List<String> _influential;

		public static List<String> PersuasiveSell => _persuasiveSell ?? (_persuasiveSell = new List<String>()
		{
			"You", "Easy", "Money", "Save", "Love", "New", "Discovery", "Results", "Proven",
			"Health", "Guarantee"
		});
		private static List<String> _persuasiveSell;


		public static List<String> Community => _community ?? (_community = new List<String>()
		{
			"Join", "Become", "Member", "Come along", "Sign up"
		});
		private static List<String> _community;

		public static List<String> Exclusive => _exclusive ?? (_exclusive = new List<String>()
		{
			"Members only", "Sign up required", "Login required", "Caused by", "Class full", "Membership now closed", "Ask for an invitation", 
			"Apply to be one of our beta testers", "Exclusive offers", "Become an insider", "Be one of the few", "Get it before everybody else",
			"Be the first to hear about it", "Only available to subscribers"
		});
		private static List<String> _exclusive;

		public static List<String> Scarcity => _scarcity ?? (_scarcity = new List<String>()
		{
			"Limited offer", "Supplies running out", "Get them while they last", 
			"Sale ends soon", "Today only", "Only 10 available", "Only 3 left",
			"Only available here", "Only available to subscribers"
		});
		private static List<String> _scarcity;

		public static List<String> FeelSafe => _feelSafe ?? (_feelSafe = new List<String>()
		{
			"Anonymous", "Authentic", "Backed", "Best-selling", "Cancel Anytime", "Certified", "Endorsed",
			"Guaranteed", "Ironclad", "Lifetime", "Moneyback", "No Obligation", "No Questions Asked", "No Risk",
			"No Strings Attached", "Official", "Privacy", "Protected", "Proven", "Recession-proof", "Refund",
			"Research", "Results", "Secure", "Tested", "Try before You Buy", "Verify", "Unconditional"
		});
		private static List<String> _feelSafe;

		public static List<String> Testimonial => _testimonial ?? (_testimonial = new List<String>()
		{
			"Improve", "Trust", "Immediately", "Discover", "Profit", "Learn", "Know",
			"Understand", "Powerful", "Best", "Win", "Hot Special", "More", "Bonus",
			"Exclusive", "Extra", "You", "Free", "Health", "Guarantee", "New",
			"Proven", "Safety", "Money", "Now", "Today", "Results", "Protect",
			"Help", "Easy", "Amazing", "Latest", "Extraordinary", "How to", "Worst",
			"Ultimate", "Hot", "First", "Big", "Anniversary", "Premiere", "Basic",
			"Complete", "Save", "Plus!", "Create"
		});
		private static List<String> _testimonial;

		public static List<String> Headline => _headline ?? (_headline = new List<String>()
		{
			"Secret", "Tell us", "Inspires", "Take", "Help", "Promote", "Increase",
			"Create", "Discover"
		});
		private static List<String> _headline;

	}
}
