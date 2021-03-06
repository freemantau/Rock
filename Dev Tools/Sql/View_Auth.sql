
SELECT  
       a.[Id]
      ,et.Name as EntityTypeName
      ,case [et].[Guid] 
       when '65CDFD5B-A9AA-48FA-8D22-669612D5EA7D' then (select top 1 ClassName from RestController where Id = a.EntityId) 
       when 'D4F7F055-5351-4ADF-9F8D-4802CAD6CC9D' then (select top 1 ra.ApiId from RestAction ra where Id = a.EntityId) 
       else convert(nvarchar(max), [EntityId])
      end [Entity.Name]
      ,[a].[Order]
      ,[Action]
      ,[AllowOrDeny]
      ,[SpecialRole]
      ,[PersonAliasId]
      ,g.Name as GroupName
      ,a.[Guid]
      
  FROM [Auth] a
  left outer join EntityType et on et.Id = a.EntityTypeId
  left outer join [Group] g on g.Id = a.GroupId
  
  order by a.ModifiedDateTime desc
  --order by et.Name, a.[Order]