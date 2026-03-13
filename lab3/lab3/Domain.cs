using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;

namespace lab3
{
    /// <summary>
    /// Base sports record that can be serialized to BSON.
    /// </summary>
    public abstract class MediaItem
    {
        /// <summary>
        /// Display name of the sports record (team, athlete or event).
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Year that is important for this record (season, event year, etc.).
        /// </summary>
        public int Year { get; set; }

        /// <summary>
        /// Coach or main responsible person.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Gets the textual type name to show in the UI.
        /// </summary>
        public abstract string TypeName { get; }

        /// <summary>
        /// Returns additional info specific for a concrete type.
        /// </summary>
        /// <returns>Details string.</returns>
        public abstract string GetDetails();

        /// <summary>
        /// Returns a human readable representation for list boxes.
        /// </summary>
        /// <returns>Item string.</returns>
        public override string ToString()
        {
            return $"{TypeName}: {Title} ({Year}) - {Creator}";
        }
    }

    /// <summary>
    /// Football sports record.
    /// </summary>
    public class Football : MediaItem
    {
        /// <summary>
        /// League where the team or event participates.
        /// </summary>
        public string League { get; set; }

        /// <inheritdoc />
        public override string TypeName => "Football";

        /// <inheritdoc />
        public override string GetDetails()
        {
            return $"League: {League}";
        }
    }

    /// <summary>
    /// Basketball sports record.
    /// </summary>
    public class Basketball : MediaItem
    {
        /// <summary>
        /// League or tournament name.
        /// </summary>
        public string League { get; set; }

        /// <inheritdoc />
        public override string TypeName => "Basketball";

        /// <inheritdoc />
        public override string GetDetails()
        {
            return $"League: {League}";
        }
    }

    /// <summary>
    /// Swimming sports record.
    /// </summary>
    public class Swimming : MediaItem
    {
        /// <summary>
        /// Distance of the swimming event in meters.
        /// </summary>
        public int DistanceMeters { get; set; }

        /// <inheritdoc />
        public override string TypeName => "Swimming";

        /// <inheritdoc />
        public override string GetDetails()
        {
            return $"Distance: {DistanceMeters} m";
        }
    }

    /// <summary>
    /// Running sports record.
    /// </summary>
    public class Running : MediaItem
    {
        /// <summary>
        /// Distance of the running event in kilometers.
        /// </summary>
        public double DistanceKm { get; set; }

        /// <inheritdoc />
        public override string TypeName => "Running";

        /// <inheritdoc />
        public override string GetDetails()
        {
            return $"Distance: {DistanceKm:0.##} km";
        }
    }

    /// <summary>
    /// Tennis sports record.
    /// </summary>
    public class Tennis : MediaItem
    {
        /// <summary>
        /// Court surface type (clay, grass, hard etc.).
        /// </summary>
        public string Surface { get; set; }

        /// <inheritdoc />
        public override string TypeName => "Tennis";

        /// <inheritdoc />
        public override string GetDetails()
        {
            return $"Surface: {Surface}";
        }
    }

    /// <summary>
    /// Cycling sports record.
    /// </summary>
    public class Cycling : MediaItem
    {
        /// <summary>
        /// Distance of the cycling race in kilometers.
        /// </summary>
        public int DistanceKm { get; set; }

        /// <inheritdoc />
        public override string TypeName => "Cycling";

        /// <inheritdoc />
        public override string GetDetails()
        {
            return $"Distance: {DistanceKm} km";
        }
    }

    /// <summary>
    /// Descriptor that knows how to create a concrete sports record for the UI.
    /// New types are added by creating a new descriptor instance.
    /// </summary>
    public sealed class MediaItemTypeDescriptor
    {
        /// <summary>
        /// Display name in the type combo box.
        /// </summary>
        public string Name { get; }

        private readonly Func<string, string, int, MediaItem> factory;

        /// <summary>
        /// Initializes a new descriptor.
        /// </summary>
        public MediaItemTypeDescriptor(string name, Func<string, string, int, MediaItem> factory)
        {
            Name = name;
            this.factory = factory;
        }

        /// <summary>
        /// Creates a new media item instance using common parameters.
        /// </summary>
        public MediaItem Create(string title, string creator, int year)
        {
            return factory(title, creator, year);
        }

        /// <summary>
        /// Returns the descriptor name for UI elements.
        /// </summary>
        public override string ToString()
        {
            return Name;
        }
    }

    /// <summary>
    /// Registry of known sports record types.
    /// Adding a new type is done by adding a descriptor only.
    /// </summary>
    public static class MediaItemTypeRegistry
    {
        private static readonly List<MediaItemTypeDescriptor> types = new List<MediaItemTypeDescriptor>
        {
            new MediaItemTypeDescriptor("Football", (title, creator, year) => new Football
            {
                Title = title,
                Creator = creator,
                Year = year,
                League = "Premier League"
            }),
            new MediaItemTypeDescriptor("Basketball", (title, creator, year) => new Basketball
            {
                Title = title,
                Creator = creator,
                Year = year,
                League = "NBA"
            }),
            new MediaItemTypeDescriptor("Swimming", (title, creator, year) => new Swimming
            {
                Title = title,
                Creator = creator,
                Year = year,
                DistanceMeters = 100
            }),
            new MediaItemTypeDescriptor("Running", (title, creator, year) => new Running
            {
                Title = title,
                Creator = creator,
                Year = year,
                DistanceKm = 5.0
            }),
            new MediaItemTypeDescriptor("Tennis", (title, creator, year) => new Tennis
            {
                Title = title,
                Creator = creator,
                Year = year,
                Surface = "Hard"
            }),
            new MediaItemTypeDescriptor("Cycling", (title, creator, year) => new Cycling
            {
                Title = title,
                Creator = creator,
                Year = year,
                DistanceKm = 40
            })
        };

        /// <summary>
        /// Exposes registered types as read-only list.
        /// </summary>
        public static IReadOnlyList<MediaItemTypeDescriptor> Types => types;
    }

    /// <summary>
    /// Wrapper around BindingList that encapsulates BSON serialization logic.
    /// </summary>
    public class MediaItemList
    {
        /// <summary>
        /// Static constructor registers class maps for polymorphic BSON serialization.
        /// </summary>
        static MediaItemList()
        {
            // Register mappings only once per application domain.
            if (!BsonClassMap.IsClassMapRegistered(typeof(MediaItem)))
            {
                BsonClassMap.RegisterClassMap<MediaItem>(cm =>
                {
                    cm.AutoMap();
                    cm.SetIsRootClass(true);
                });

                BsonClassMap.RegisterClassMap<Football>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<Basketball>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<Swimming>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<Running>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<Tennis>(cm => { cm.AutoMap(); });
                BsonClassMap.RegisterClassMap<Cycling>(cm => { cm.AutoMap(); });
            }
        }

        /// <summary>
        /// Items that can be bound directly to Windows Forms controls.
        /// </summary>
        public BindingList<MediaItem> Items { get; } = new BindingList<MediaItem>();

        /// <summary>
        /// Adds a new item to the collection.
        /// </summary>
        public void Add(MediaItem item)
        {
            Items.Add(item);
        }

        /// <summary>
        /// Removes an item from the collection.
        /// </summary>
        public void Remove(MediaItem item)
        {
            Items.Remove(item);
        }

        /// <summary>
        /// Serializes current collection to a BSON file.
        /// </summary>
        /// <param name="filePath">Target file path.</param>
        public void SaveToBson(string filePath)
        {
            var array = new BsonArray();
            foreach (var item in Items)
            {
                // Rely on MongoDB driver to include type discriminator for derived types.
                var document = item.ToBsonDocument();
                array.Add(document);
            }

            var bytes = array.ToBson();
            File.WriteAllBytes(filePath, bytes);
        }

        /// <summary>
        /// Loads collection from a BSON file, replacing current items.
        /// </summary>
        /// <param name="filePath">Source file path.</param>
        public void LoadFromBson(string filePath)
        {
            if (!File.Exists(filePath))
            {
                throw new FileNotFoundException("File for deserialization was not found.", filePath);
            }

            var bytes = File.ReadAllBytes(filePath);
            var array = BsonSerializer.Deserialize<BsonArray>(bytes);

            Items.Clear();
            foreach (var value in array)
            {
                if (value is BsonDocument doc)
                {
                    var item = BsonSerializer.Deserialize<MediaItem>(doc);
                    Items.Add(item);
                }
            }
        }
    }
}

