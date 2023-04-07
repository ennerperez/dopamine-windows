using Amphetamine.Core.Enums;

namespace Amphetamine.Core.Models
{
	public class Lyrics
	{
		private string _text;
		private string source;

		public string Text
		{
			get { return _text; }
			set { _text = value; }
		}

		public string Source
		{
			get { return source; }
		}

		public bool HasText
		{
			get { return !string.IsNullOrWhiteSpace(Text); }
		}

		public bool HasSource
		{
			get { return !string.IsNullOrWhiteSpace(source); }
		}

		public Source SourceType { get; set; }

		public Lyrics()
		{
			_text = string.Empty;
			source = string.Empty;
		}

		public Lyrics(string text, string source)
		{
			_text = text;
			this.source = source;
		}

		public Lyrics(string text, string source, Source sourceType) : this(text, source)
		{
			SourceType = sourceType;
		}

		public Lyrics Clone()
		{
			return new Lyrics(_text, Source, SourceType);
		}
	}
}
