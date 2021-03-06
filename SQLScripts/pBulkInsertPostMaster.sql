
alter  PROCEDURE [dbo].pBulkInsertPostMaster  
(  
 @Action Nvarchar(200),  
 @JParamVal Nvarchar(Max),  
 @UserId nvarchar(250),
 @Message Nvarchar(1000) OUT   
)  
As  
BEGIN  
declare @PageId varchar(60)
set @PageId=(select PageId From UserPages where UserId=@UserId)
 SELECT * into #TempPMMaster  
 FROM OPENJSON (@JParamVal, N'$')  
 WITH (  
		FacebookPost nvarchar (200) N'$.FacebookPost', 
		PageName nvarchar (200) N'$.PageName', 
		Message nvarchar (200) N'$.Message', 
		ImageUrl nvarchar (200) N'$.ImageUrL', 
		ScheduledTime datetime N'$.ScheduledTime' 
 ) AS JsonRIMaster;  
  

insert into SchedulePosts(FacebookPost,PageId,PageName,Message,ImageUrl,ClientId,ScheduledTime,IsPosted,InProcess,IsVideo)
select TT.FacebookPost,@PageId,TT.PageName,TT.Message,'https://social.alldealz.ca/api/files?file='+TT.ImageUrl+'' as ImageUrl,@UserId,TT.ScheduledTime,0,0,1 from #TempPMMaster TT

 
  Set @Message ='Inserted Successfully'

END  

