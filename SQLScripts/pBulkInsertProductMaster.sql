USE [ProjectN_Testing]
GO
/****** Object:  StoredProcedure [dbo].[pBulkInsertProductMaster]    Script Date: 21-08-2021 00:04:04 ******/
SET ANSI_NULLS ON
GO
SET QUOTED_IDENTIFIER ON
GO

ALTER PROCEDURE [dbo].[pBulkInsertProductMaster]  
(  
 @Action Nvarchar(200),  
 @JParamVal Nvarchar(Max),  
 @UserId bigint,
 @Message Nvarchar(1000) OUT   
)  
As  
BEGIN  
								

 SELECT * into #TempPMMaster  
 FROM OPENJSON (@JParamVal, N'$')  
 WITH (  
		Name nvarchar (200) N'$.Name', 
		CategoryName nvarchar (200) N'$.CategoryName', 
		Description nvarchar (40) N'$.Description', 
		RegularPrice decimal(18,2) N'$.RegularPrice', 
		TaxType nvarchar (40) N'$.TaxType', 
		
		IsRecommended bit N'$.IsRecommended', 
		IsActive bit N'$.IsActive', 
		Email nvarchar (60) N'$.Email', 
		Image nvarchar(140)  N'$.image'
		
 ) AS JsonRIMaster;  
  
  declare @EId nvarchar(50)
  set @EId=(select Top 1 Email from #TempPMMaster)
  set @UserId=(select id from Users where UserName=@EId)
INSERT INTO Product(Name,CategoryId,Description,image,UserId,RegularPrice,TaxType,IsRecommended,IsActive)
select T.Name,C.Id,T.Description,T.image,@UserId,T.RegularPrice,T.TaxType,T.IsRecommended,T.IsActive from #TempPMMaster T
INNER JOIN Category c ON c.Name=T.CategoryName and C.UserId=@UserId
 
     
 
  Set @Message ='Inserted Successfully'

END  

