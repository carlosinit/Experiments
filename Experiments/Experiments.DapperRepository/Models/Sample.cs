using System;

namespace Experiments.DapperRepository.Models
{
    public class Sample
    {
        public int Id { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }

        private Sample()
        {
        }

        public Sample(int id, string name, string description)
            : this(name, description)
        {
            if (id <= 0) throw new ArgumentOutOfRangeException(nameof(id));
            Id = id;
        }

        public Sample(string name, string description)
        {
            if (string.IsNullOrWhiteSpace(name)) throw new ArgumentNullException(nameof(name));
            if (string.IsNullOrWhiteSpace(description)) throw new ArgumentNullException(nameof(description));

            Name = name;
            Description = description;
        }
    }
}