﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Dopamine.Data.Entities
{
    public class AlbumArtwork
    {
        //TODO:
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public long AlbumArtworkID { get; set; }

        public string AlbumKey { get; set; }

        public string ArtworkID { get; set; }

        public AlbumArtwork()
        {

        }

        public AlbumArtwork(string AlbumKey)
        {
            this.AlbumKey = AlbumKey;
        }

        public override bool Equals(object obj)
        {
            if (obj == null || !GetType().Equals(obj.GetType()))
            {
                return false;
            }

            return this.AlbumKey.Equals(((AlbumArtwork)obj).AlbumKey);
        }

        public override int GetHashCode()
        {
            return new { this.AlbumKey }.GetHashCode();
        }
    }
}
