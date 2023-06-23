
-- gets the list of open activities without a single record within the last 18 months

SELECT
  CONCAT('#', LPAD(subprojects.ProjectNumber, 4, 0), '.', LPAD(subprojects.Number, 3, 0), '.', LPAD(activities.Number, 2, 0)) AS Activity,
  subprojects.CustomerName,
  subprojects.ProjectName,
  subprojects.Name AS SubprojectName,
  activities.Name AS ActivityName,
  CONCAT(users.FirstName, ' ', users.LastName) AS Manager,
  lastRecords.LastRecordDate,
  lastRecords.LimitDate

FROM activities
JOIN subprojects ON subprojects.Id = activities.SubprojectId
JOIN users ON users.Id = subprojects.ManagerId
INNER JOIN
(
  SELECT
    recordentries.ActivityId AS ActivityId,
    max(records.Date) AS LastRecordDate,
    (CURDATE() - INTERVAL 18 MONTH) AS LimitDate

  FROM recordentries
  JOIN records ON records.Id = recordentries.RecordId
  GROUP BY recordentries.ActivityId
) lastRecords ON lastRecords.ActivityId = activities.Id

WHERE activities.EndedDate IS NULL
 AND lastRecords.LastRecordDate < lastRecords.LimitDate
 AND subprojects.CustomerName <> 'M&F'

ORDER BY subprojects.ProjectNumber, subprojects.Number, activities.Number
