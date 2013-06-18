using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using Vintage.IO;

namespace Vintage.Films
{
    public class Halo4Film : IHaloFilm, INotifyPropertyChanged
    {
        // Private Stuff
        private IList<PlayerEntry> _players;

        // Public Stuff
        public string FilmPath { get; private set; }
        public IList<PlayerEntry> Players
        {
            get { return _players; }
            set { _players = value; NotifyPropertyChanged("Players"); }
        }

        // Classes
        public class PlayerEntry
        {
            public PlayerEntry(IReader reader)
            {
                var basepos = reader.Position;

                // Gamertag Entry 1
                GamerTag_1 = reader.ReadUTF16();

                // Service Tag
                reader.SeekTo(basepos + 0x44);
                ServiceTag = reader.ReadUTF16();

                // Gamertag Entry 2
                reader.SeekTo(basepos + 0x190);
                GamerTag_2 = reader.ReadUTF16();
            }

            public string GamerTag_1 { get; set; }
            public string ServiceTag { get; set; }
            public string GamerTag_2 { get; set; }
        }

        public void Load(string filePath)
        {
            // Open Film
            FilmPath = filePath;
            var stream = new EndianReader(new FileStream(FilmPath, FileMode.Open), Endian.BigEndian);

            ValidateFilm(stream);

            LoadPlayerTable(stream);

            stream.Close();
        }

        private void ValidateFilm(IReader reader)
        {
            // Validate Header Magic
            reader.SeekTo(0x00);
            var test = reader.ReadAscii();
            if (test != "_blf")
            {
                reader.Close();
                throw new NotSupportedException("Unsupported file.");
            }

            // Validate Header Type
            reader.SeekTo(0x0E);
            test = reader.ReadAscii();
            if (test != "reach saved film")
            {
                reader.Close();
                throw new NotSupportedException("Unsupported file.");
            }

            // Validate BLF Film Chunk Header
            reader.SeekTo(0x340);
            test = reader.ReadAscii();
            if (test != "flmh")
            {
                reader.Close();
                throw new NotSupportedException("Unsupported file.");
            }
        }
        private void LoadPlayerTable(IReader reader)
        {
            var chainedNullEntries = 0;
            var index = 0;
            _players = new List<PlayerEntry>();
            while (chainedNullEntries < 2)
            {
                reader.SeekTo(0x2BBF0 + (0x1E0 * index));
                var entry = new PlayerEntry(reader);
                if (entry.GamerTag_1 == "" || entry.ServiceTag == "" || entry.GamerTag_2 == "")
                    chainedNullEntries++;
                else
                {
                    _players.Add(entry);
                    chainedNullEntries = 0;
                }
                index++;
            }
        }

        public void Save()
        {
            throw new NotImplementedException();
        }

        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string info)
        {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(info));
        }
    }
}
