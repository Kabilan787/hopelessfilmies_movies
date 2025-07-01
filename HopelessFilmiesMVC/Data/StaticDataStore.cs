using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HopelessFilmiesMVC.Data;

//public class Film
//{
//    public int Id { get; set; }
//    public string Heading { get; set; }
//    public string Description { get; set; }
//    public string Link { get; set; }
//    public string Image { get; set; }
//}

public class Film
{
    public int Id { get; set; }
    public string Heading { get; set; }
    public string Description { get; set; }
    public string Link { get; set; }
    public string Image { get; set; }
    public string Language { get; set; }
    public int Year { get; set; }
    public string GenreString { get; set; }  // Store as string in DB
    [NotMapped]
    public List<string> Genre
    {
        get => string.IsNullOrEmpty(GenreString)
            ? new List<string>()
            : GenreString.Split(',').Select(g => g.Trim()).ToList();
        set => GenreString = string.Join(", ", value);
    }
    public string Writer { get; set; }
    public string Director { get; set; }
    public string StarsString { get; set; }  // Store as string in DB
    [NotMapped]
    public List<string> Stars
    {
        get => string.IsNullOrEmpty(StarsString)
            ? new List<string>()
            : StarsString.Split(',').Select(s => s.Trim()).ToList();
        set => StarsString = string.Join(", ", value);
    }
    public string Category { get; set; }  // shortfilms or movies
}


public class Podcast
{
    [Key]
    public int Id { get; set; }

    public string Heading { get; set; }

    public string Description { get; set; }

    public string Host { get; set; }

    // Store guest names as comma-separated string
    public string GuestsString { get; set; }

    [NotMapped]
    public List<string> Guests
    {
        get => string.IsNullOrEmpty(GuestsString) ? new List<string>() : GuestsString.Split(',').Select(g => g.Trim()).ToList();
        set => GuestsString = string.Join(", ", value);
    }

    public string Language { get; set; }

    public int Year { get; set; }

    public string Duration { get; set; }

    public string Link { get; set; }

    public string Image { get; set; }

    // Store tags (or genres) as comma-separated string
    public string GenreString { get; set; }

    [NotMapped]
    public List<string> Genre
    {
        get => string.IsNullOrEmpty(GenreString) ? new List<string>() : GenreString.Split(',').Select(t => t.Trim()).ToList();
        set => GenreString = string.Join(", ", value);
    }
}

public class TeamMember
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Image { get; set; }
    public List<string> Roles { get; set; }
}

public class ContactInfo
{
    public string IconClass { get; set; }
    public string Text { get; set; }
    public bool IsLink { get; set; }
    public string Link { get; set; }
}

public class JsonData
{
    public List<Film> Shortfilms { get; set; }   // ✅ uses Film model
    public List<Film> Movies { get; set; }       // ✅ also uses Film model
    public List<Podcast> Podcasts { get; set; }
    public List<TeamMember> TeamMembers { get; set; }
    public List<ContactInfo> ContactInfo { get; set; }
}


public static class StaticDataStore
{
    //public static List<Film> Shortfilms;
    //public static List<Film> Movies;
    //public static List<Podcast> Podcasts;
    public static List<TeamMember> TeamMembers;
    public static List<ContactInfo> ContactInfos;

    static StaticDataStore()
    {
        string jsonFilePath = "wwwroot/data/data.json"; 

        
        string jsonString = File.ReadAllText(jsonFilePath);
        JsonSerializerOptions options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true, // Handle different casing in JSON
                Converters = { new JsonStringEnumConverter() } // Handle enums if any
            };

        JsonData data = JsonSerializer.Deserialize<JsonData>(jsonString, options);

        //Shortfilms = data.Shortfilms ?? new List<Film>(); 
        //Movies = data.Movies ?? new List<Film>();        
        //Podcasts = data.Podcasts ?? new List<Podcast>();
        TeamMembers = data.TeamMembers ?? new List<TeamMember>();
        ContactInfos = data.ContactInfo ?? new List<ContactInfo>();


    }
}