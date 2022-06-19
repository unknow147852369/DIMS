/****** Script for SelectTopNRows command from SSMS  ******/
 INSERT INTO [dbo].[Hotels] (
      [HotelName]
      ,[HotelAddress]
      ,[UserID]
      ,[District]/****** Fix han nay ******/
      ,[Province]/****** Fix han nay ******/
      ,[CreateDate]
      ,[Status]
      ,[TotalRate])
VALUES (
N'Kally Hotel',
N'47 Hoàng Diệu, Phường 12, Quận 4, Hồ Chí Minh, Việt Nam',
7,
773,
79,
'2022-06-16 00:00:00.000',
1,
8)

SELECT TOP (1000) [HotelID]
      ,[HotelName]
      ,[HotelNameNoMark]
      ,[HotelAddress]
      ,[UserID]
      ,[Ward]
      ,[District]
      ,[Province]
      ,[CreateDate]
      ,[Status]
      ,[TotalRate]
  FROM [dbo].[Hotels]
  /****** Script for hotel photo command from SSMS  ******/



    insert into [dbo].[Photos](
      [HotelID] /****** Fix han nay ******/
      ,[PhotoUrl]/****** Fix han nay ******/
      ,[CreateDate]
      ,[IsMain]
      ,[Status]
  )
  VALUES 
 (1038,N'https://picsum.photos/1920/1080?random=1151','2022-06-16 00:00:00.000','true', 1 ),
(1038,N'https://picsum.photos/1920/1080?random=12151','2022-06-16 00:00:00.000','false',1),
 (1038,N'https://picsum.photos/1920/1080?random=16513','2022-06-16 00:00:00.000','false',1),
  (1038,N'https://picsum.photos/1920/1080?random=99511','2022-06-16 00:00:00.000','false',1),
   (1038,N'https://picsum.photos/1920/1080?random=9511','2022-06-16 00:00:00.000','false',1)


   SELECT TOP (1000) [PhotoID]
      ,[HotelID]
      ,[CategoryID]
      ,[PhotoUrl]
      ,[CreateDate]
      ,[IsMain]
      ,[Status]
  FROM [dbo].[Photos]
/****** Script for SelectTopNRows command from SSMS  ******/


  insert into [dbo].[Categories](
	  [HotelID]/****** Fix han nay ******/
      ,[CategoryName]
      ,[Quanity]
      ,[Status]
  )
  VALUES 
  (1038,N'Single bedroom',1,1),
(1038,N'Double bedroom',2,1),
  (1038,N'Triple bedroom',3,1)


  SELECT TOP (1000) [CategoryID]
      ,[HotelID]
      ,[CategoryName]
      ,[CateDescrpittion]
      ,[Quanity]
      ,[Status]
  FROM [dbo].[Categories]
  /****** Script for category photo command from SSMS  ******/


    insert into [dbo].[Photos](
      [CategoryID]/****** Fix han nay ******/
      ,[PhotoUrl]/****** Fix han nay ******/
      ,[CreateDate]
      ,[IsMain]
      ,[Status]
  )
  VALUES 
 (47,N'https://picsum.photos/1920/1080?random=949','2022-06-16 00:00:00.000','true', 1 ),
 (47,N'https://picsum.photos/1920/1080?random=94449','2022-06-16 00:00:00.000','false', 1 ),
 (47,N'https://picsum.photos/1920/1080?random=9449','2022-06-16 00:00:00.000','false', 1 ),
 (48,N'https://picsum.photos/1920/1080?random=96419','2022-06-16 00:00:00.000','true',1),
 (48,N'https://picsum.photos/1920/1080?random=957439','2022-06-16 00:00:00.000','false',1),
 (48,N'https://picsum.photos/1920/1080?random=947469','2022-06-16 00:00:00.000','false',1),
 (49,N'https://picsum.photos/1920/1080?random=937469','2022-06-16 00:00:00.000','true',1),
 (49,N'https://picsum.photos/1920/1080?random=927499','2022-06-16 00:00:00.000','false',1),
 (49,N'https://picsum.photos/1920/1080?random=914799','2022-06-16 00:00:00.000','false',1)

 SELECT TOP (1000) [PhotoID]
      ,[HotelID]
      ,[CategoryID]
      ,[PhotoUrl]
      ,[CreateDate]
      ,[IsMain]
      ,[Status]
  FROM [dbo].[Photos]
  /****** Script for SelectTopNRows command from SSMS  ******/

  insert into [dbo].[Rooms]([RoomName]
      ,[HotelID]/****** Fix han nay ******/
      ,[CategoryID]/****** Fix han nay ******/
      ,[Price]
      ,[Status])
  values
  ('1',1016,47,1000,1),
  ('2',1016,47,1000,1),
  ('3',1016,47,2000,1),
  ('4',1016,48,3000,1),
  ('5',1016,48,4000,1),
  ('6',1016,48,5000,1),
  ('7',1016,49,6000,1),
  ('8',1016,49,7000,1),
  ('9',1016,49,8000,1)
  
  SELECT TOP (1000) [RoomID]
      ,[RoomName]
      ,[HotelID]
      ,[CategoryID]
      ,[RoomDescription]
      ,[Price]
      ,[Status]
  FROM [dbo].[Rooms]

