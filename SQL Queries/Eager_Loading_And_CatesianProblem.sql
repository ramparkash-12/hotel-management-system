-- Before
/*
SELECT t.Id, t.Address, t.City, t.Country, t.Description,
 t.FeaturedFrom, t.FeaturedTo, t.IsFeatured, t.Name, t.Stars,
  t.Status, i.Id, i.Extension, i.HotelImageId, i.ImageType,
   i.Name, i.RoomImageId, i.Size, i.URI, r.Id, r.Available, 
   r.Description, r.HotelId, r.MaximumGuests, r.Number, r.Price, r.RoomTypeId
    FROM 
    (
        SELECT TOP(1) h.Id, h.Address, h.City, h.Country, h.Description, h.FeaturedFrom, h.FeaturedTo, h.IsFeatured, h.Name, h.Stars, h.Status
        FROM Hotels AS h
        WHERE (h.Id = 66) AND (h.Id = 66)
    ) AS t
LEFT JOIN Images AS i ON t.Id = i.HotelImageId
LEFT JOIN Rooms AS r ON t.Id = r.HotelId
ORDER BY t.Id ASC, i.Id ASC, r.Id ASC
*/

-- After
-- 1: Get Hotel 
SELECT [t].[Id], [t].[Address], [t].[City], [t].[Country], [t].[Description],
 [t].[FeaturedFrom], [t].[FeaturedTo], [t].[IsFeatured], [t].[Name], [t].[Stars], [t].[Status]
FROM (
    SELECT TOP(1) [h].[Id], [h].[Address], [h].[City], [h].[Country], [h].[Description],
     [h].[FeaturedFrom], [h].[FeaturedTo], [h].[IsFeatured], [h].[Name], [h].[Stars], [h].[Status]
    FROM [Hotels] AS [h]
    WHERE ([h].[Id] = 66) AND ([h].[Id] = 66)
) AS [t]
ORDER BY [t].[Id]

-- 2: Get Hotel with Image

SELECT [i].[Id], [i].[Extension], [i].[HotelImageId], [i].[ImageType],
 [i].[Name], [i].[RoomImageId], [i].[Size], [i].[URI], [t].[Id]
FROM (
    SELECT TOP(1) [h].[Id], [h].[Address], [h].[City], [h].[Country], [h].[Description], 
    [h].[FeaturedFrom], [h].[FeaturedTo], [h].[IsFeatured], [h].[Name], [h].[Stars], [h].[Status]
    FROM [Hotels] AS [h]
    WHERE ([h].[Id] = 66) AND ([h].[Id] = 66)
) AS [t]
INNER JOIN [Images] AS [i] ON [t].[Id] = [i].[HotelImageId]
ORDER BY [t].[Id]

-- 3: Get Room with Image

SELECT [r].[Id], [r].[Available], [r].[Description], [r].[HotelId], 
[r].[MaximumGuests], [r].[Number], [r].[Price], [r].[RoomTypeId], [t].[Id]
FROM (
    SELECT TOP(1) [h].[Id], [h].[Address], [h].[City], [h].[Country], 
    [h].[Description], [h].[FeaturedFrom], [h].[FeaturedTo],
    [h].[IsFeatured], [h].[Name], [h].[Stars], [h].[Status]
    FROM [Hotels] AS [h]
    WHERE ([h].[Id] = 66) AND ([h].[Id] = 66)
) AS [t]
INNER JOIN [Rooms] AS [r] ON [t].[Id] = [r].[HotelId]
ORDER BY [t].[Id]


-- 3: Get Hotel with Room  Then Include images of Room . . . . . . . .

SELECT [i].[Id], [i].[Extension], [i].[HotelImageId], [i].[ImageType],
 [i].[Name], [i].[RoomImageId], [i].[Size], [i].[URI], [t].[Id], [r].[Id]
FROM (
    SELECT TOP(1) [h].[Id], [h].[Address], [h].[City], [h].[Country],
     [h].[Description], [h].[FeaturedFrom], [h].[FeaturedTo], [h].[IsFeatured], [h].[Name], [h].[Stars], [h].[Status]
    FROM [Hotels] AS [h]
    WHERE ([h].[Id] = 66) AND ([h].[Id] = 66)
) AS [t]
INNER JOIN [Rooms] AS [r] ON [t].[Id] = [r].[HotelId]
INNER JOIN [Images] AS [i] ON [r].[Id] = [i].[RoomImageId]
ORDER BY [t].[Id], [r].[Id]

