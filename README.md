# Soundify MusicService

## Overview

**Soundify MusicService** is a platform designed to manage and store metadata about music, artists, albums, tracks, playlists, genres, ratings, and user interactions. It integrates with the **IdentityCore** authentication service for secure user management and offers flexible content management for different roles, such as **SuperAdmin**, **Admin**, and **Publisher**. The service stores music files, album artwork, and other related assets in an external **StorageService** and provides robust features for content creation, editing, and curation.

---

## Features

### Artists

- Manage artist profiles and their associated social media links.

### Tracks

- Store information about individual music tracks, including ratings, duration, and associated album.

### Albums

- Organize tracks into albums, with metadata such as release date and cover art.

### Playlists

- Create and manage playlists, allowing users to group tracks together.

### Genres

- Categorize music by genre.

### Ratings & Favorites

- Enable users to rate tracks and mark them as favorites.

### Content Management

- Role-based permissions for content curation and editing.

---

## Dependencies

- **IdentityCore**: The service relies on the **[IdentityCore](https://github.com/Isukaza/IdentityCore)** for user authentication and authorization.
- **StorageService**: Music files and images are stored externally via the **StorageService** URL.

---

## Data Management

### Database

**PostgreSQL** serves as the primary storage for metadata regarding artists, tracks, albums, playlists, and user interactions, consisting of the following main tables:

#### 1. Artists

Stores details about music artists, including:
- `Id`: Unique identifier for the artist.
- `Name`: Name of the artist.
- `ImageFilePath`: Path to the artist's image file.
- `Created`: Timestamp when the artist was created.
- `Modified`: Timestamp for the last modification of the artist's details.
- `PublisherId`: Foreign key linking the artist to the publisher.

#### 2. Tracks

Stores information about individual music tracks, including:
- `Id`: Unique identifier for the track.
- `Title`: Title of the track.
- `Duration`: Duration of the track in seconds.
- `ReleaseDate`: Timestamp when the track was released.
- `FilePath`: Path to the audio file for the track.
- `TotalRating`: Overall rating of the track (average rating from users).
- `RatingCount`: Number of times the track has been rated.
- `AlbumId`: Foreign key linking the track to its album (can be null).
- `GenreId`: Foreign key linking the track to its genre.
- `Created`: Timestamp when the track was created.
- `Modified`: Timestamp for the last modification of the track.

#### 3. Albums

Stores details about music albums, including:
- `Id`: Unique identifier for the album.
- `Title`: Title of the album.
- `ReleaseDate`: Release date and time of the album.
- `CoverFilePath`: Path to the album's cover image.
- `ArtistId`: Foreign key linking the album to its artist.
- `Created`: Timestamp when the album was created.
- `Modified`: Timestamp for the last modification of the album.

#### 4. Playlists

Stores information about user-generated playlists, including:
- `Id`: Unique identifier for the playlist.
- `UserId`: Foreign key linking the playlist to the user who created it.
- `Title`: Title of the playlist.
- `Description`: Optional description of the playlist.
- `Created`: Timestamp when the playlist was created.
- `Modified`: Timestamp for the last modification of the playlist.

#### 5. Genres

Stores the different music genres, including:
- `Id`: Unique identifier for the genre.
- `Name`: Name of the genre (e.g., Rock, Pop).
- `Created`: Timestamp when the genre was created.
- `Modified`: Timestamp for the last modification of the genre.

#### 6. PlaylistTracks

Represents the many-to-many relationship between playlists and tracks, including:
- `Id`: Unique identifier for the playlist-track relationship.
- `PlaylistId`: Foreign key linking the track to its playlist.
- `TrackId`: Foreign key linking the track to the playlist.
- `Created`: Timestamp when the relationship was created.
- `Modified`: Timestamp for the last modification of the relationship.

#### 7. ArtistSocialMedias

Stores the social media profiles of artists, including:
- `Id`: Unique identifier for the social media link.
- `Platform`: Identifies the social media platform (e.g., 1 for Twitter, 2 for Instagram).
- `Url`: URL of the artist's social media profile.
- `ArtistId`: Foreign key linking the social media to the artist.
- `Created`: Timestamp when the social media link was created.
- `Modified`: Timestamp for the last modification of the social media link.

#### 8. SingleTracks

Stores information about individual single tracks, including:
- `Id`: Unique identifier for the single track.
- `CoverFilePath`: Path to the cover image for the single.
- `TrackId`: Foreign key linking the single to its track.
- `ArtistId`: Foreign key linking the single to its artist.
- `Created`: Timestamp when the single track was created.
- `Modified`: Timestamp for the last modification of the single track.

#### 9. TrackRatings

Stores user ratings for tracks, including:
- `Id`: Unique identifier for the track rating.
- `Rating`: Rating value given by the user (e.g., 1 to 5 stars).
- `UserId`: Foreign key linking the rating to the user who provided it.
- `TrackId`: Foreign key linking the rating to the track.
- `Created`: Timestamp when the rating was created.
- `Modified`: Timestamp for the last modification of the rating.

#### 10. UserFavorites

Stores the favorite tracks of users, including:
- `Id`: Unique identifier for the favorite entry.
- `UserId`: Foreign key linking the favorite to the user.
- `TrackId`: Foreign key linking the favorite to the track.
- `Created`: Timestamp when the favorite was added.
- `Modified`: Timestamp for the last modification of the favorite entry.

These tables store and manage the core music metadata and user interactions within the **Soundify MusicService**. The relationships between these tables ensure efficient tracking of users, their content preferences, and the organization of music metadata.

---

## Configuration and Deployment

**Configuration and deployment settings are currently under development.** The environment variables and deployment instructions are being finalized. Please stay tuned for updates.

For more details, refer to the upcoming documentation on configuring the service, setting up the environment, and deployment steps.

---

## Swagger Documentation in Development Mode

In **Development mode**, **Swagger** is available for easier API exploration and testing. It provides:

- Examples of expected request and response data.
- XML documentation for each **endpoint** of the service.
- An interactive interface for sending API requests directly.

You can access the Swagger documentation at the following URL:

[https://localhost:8433/api/index.html](https://localhost:8433/api/index.html)

---

## Future Enhancements

For information on the project's future plans and enhancements, please refer to the [Roadmap](https://github.com/Isukaza/Soundify/blob/develop/ROADMAP.md).

---

**Soundify MusicService** is a work in progress, continually evolving to meet the demands of modern music management and content distribution.
