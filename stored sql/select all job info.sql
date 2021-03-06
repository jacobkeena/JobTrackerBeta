/****** Script for SelectTopNRows command from SSMS  ******/
SELECT j.CompanyName
		, p.JobTitle
		, j.SalaryRange
		, r.RatingDescription
		, j.CEOName
		, j.MissionStatement
		, j.Benefits
		, j.Comments
		, j.JobLink
		, l.City
		,l.CityRating
		,l.Notes
		,s.State
		,s.Capital
		,s.LargestCity
		,re.Recruiter_Name
		,re.PhoneNumber
		,re.Email
		,re.LinkedInLink
FROM Jobs j 
inner join 
Position p on p.PositionID = j.PositionID 
inner join Rating r on r.RatingID = j.RatingID 
inner join Location l on j.LocationID =l.LocationID
inner join States s on s.StateID = l.StateID
left outer join Recruiters re on re.RecruiterID = j.RecruiterID

WHERE CompanyID = 22