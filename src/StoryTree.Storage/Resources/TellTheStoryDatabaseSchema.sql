/* ---------------------------------------------------- */
/*  Generated by Enterprise Architect Version 12.0 		*/
/*  Created On : 11-Apr-2018 17:11:47 				*/
/*  DBMS       : SQLite 								*/
/* ---------------------------------------------------- */

/* Drop Tables */

DROP TABLE IF EXISTS 'HydraulicConditionElementEntity'
;

DROP TABLE IF EXISTS 'EventTreeEntity'
;

DROP TABLE IF EXISTS 'ExpertClassEstimationEntity'
;

DROP TABLE IF EXISTS 'ExpertEntity'
;

DROP TABLE IF EXISTS 'FragilityCurveElementEntity'
;

DROP TABLE IF EXISTS 'PersonEntity'
;

DROP TABLE IF EXISTS 'ProjectEntity'
;

DROP TABLE IF EXISTS 'TreeEventEntity'
;

DROP TABLE IF EXISTS 'TreeEventFragilityCurveElementEntity'
;

/* Create Tables with Primary and Foreign Keys, Check and Unique Constraints */

CREATE TABLE 'HydraulicConditionElementEntity'
(
	'HydraulicConditionElementId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'FragilityCurveElementId' INTEGER NOT NULL,
	'ProjectId' INTEGER NOT NULL,
	'WavePeriod' REAL,
	'WaveHeight' REAL,
	CONSTRAINT 'FK_HydraulicConditionElementEntity_FragilityCurveElementEntity' FOREIGN KEY ('FragilityCurveElementId') REFERENCES 'FragilityCurveElementEntity' ('FragilityCurveElementId') ON DELETE Cascade ON UPDATE Cascade,
	CONSTRAINT 'FK_HydraulicConditionElementEntity_ProjectEntity' FOREIGN KEY ('ProjectId') REFERENCES 'ProjectEntity' ('ProjectId') ON DELETE Cascade ON UPDATE Cascade
)
;

CREATE TABLE 'EventTreeEntity'
(
	'EventTreeId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'Name' TEXT,
	'Summary' TEXT,
	'Details' TEXT,
	'Color' INTEGER,
	'MainTreeEventId' INTEGER,
	'ProjectId' INTEGER NOT NULL,
	CONSTRAINT 'FK_EventTreeEntity_ProjectEntity' FOREIGN KEY ('ProjectId') REFERENCES 'ProjectEntity' ('ProjectId') ON DELETE Cascade ON UPDATE Cascade,
	CONSTRAINT 'FK_EventTreeEntity_TreeEventEntity' FOREIGN KEY ('MainTreeEventId') REFERENCES 'TreeEventEntity' ('TreeEventId') ON DELETE Cascade ON UPDATE Cascade
)
;

CREATE TABLE 'ExpertClassEstimationEntity'
(
	'ExpertClassEstimationId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'TreeEventId' INTEGER NOT NULL,
	'ExpertId' INTEGER NOT NULL,
	'WaterLevel' REAL,
	'MinEstimationId' INTEGER NOT NULL,
	'MaxEstimationId' INTEGER NOT NULL,
	'AverageEstimationId' INTEGER NOT NULL,
	CONSTRAINT 'FK_ExpertClassEstimationEntity_ExpertEntity' FOREIGN KEY ('ExpertId') REFERENCES 'ExpertEntity' ('ExpertId') ON DELETE Cascade ON UPDATE Cascade,
	CONSTRAINT 'FK_ExpertClassEstimationEntity_TreeEventEntity' FOREIGN KEY ('TreeEventId') REFERENCES 'TreeEventEntity' ('TreeEventId') ON DELETE Cascade ON UPDATE Cascade
)
;

CREATE TABLE 'ExpertEntity'
(
	'ExpertId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'PersonId' INTEGER NOT NULL,
	'Expertise' TEXT,
	'Organisation' TEXT,
	'ProjectId' INTEGER NOT NULL,
	CONSTRAINT 'FK_ExpertEntity_PersonEntity' FOREIGN KEY ('PersonId') REFERENCES 'PersonEntity' ('PersonId') ON DELETE Cascade ON UPDATE Cascade,
	CONSTRAINT 'FK_ExpertEntity_ProjectEntity' FOREIGN KEY ('ProjectId') REFERENCES 'ProjectEntity' ('ProjectId') ON DELETE Cascade ON UPDATE Cascade
)
;

CREATE TABLE 'FragilityCurveElementEntity'
(
	'FragilityCurveElementId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'WaterLevel' REAL,
	'Probability' REAL
)
;

CREATE TABLE 'PersonEntity'
(
	'PersonId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'Name' TEXT,
	'Email' TEXT,
	'Telephone' TEXT
)
;

CREATE TABLE 'ProjectEntity'
(
	'Name' TEXT,
	'Description' TEXT,
	'AssessmentSection' TEXT,
	'ProjectInformation' TEXT,
	'ProjectLeaderId' INTEGER,
	'ProjectId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	CONSTRAINT 'FK_ProjectEntity_PersonEntity' FOREIGN KEY ('ProjectLeaderId') REFERENCES 'PersonEntity' ('PersonId') ON DELETE No Action ON UPDATE No Action
)
;

CREATE TABLE 'TreeEventEntity'
(
	'TreeEventId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'Name' TEXT,
	'FailingEventId' INTEGER,
	'PassingEventId' INTEGER,
	'Details' TEXT,
	'Summary' TEXT,
	'FixedProbability' REAL,
	'ProbabilitySpecificationTypeId' INTEGER NOT NULL,
	CONSTRAINT 'FK_TreeEventEntity_FailingEventEntity' FOREIGN KEY ('FailingEventId') REFERENCES 'TreeEventEntity' ('TreeEventId') ON DELETE Cascade ON UPDATE Cascade,
	CONSTRAINT 'FK_TreeEventEntity_PassingEventEntity' FOREIGN KEY ('PassingEventId') REFERENCES 'TreeEventEntity' ('TreeEventId') ON DELETE Cascade ON UPDATE Cascade
)
;

CREATE TABLE 'TreeEventFragilityCurveElementEntity'
(
	'TreeEventFragilicyCurveElementId' INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT,
	'FragilityCurveElementId' INTEGER NOT NULL,
	'TreeEventId' INTEGER,
	CONSTRAINT 'FK_TreeEventFragilityCurveElementEntity_FragilityCurveElementEntity' FOREIGN KEY ('FragilityCurveElementId') REFERENCES 'FragilityCurveElementEntity' ('FragilityCurveElementId') ON DELETE Cascade ON UPDATE Cascade,
	CONSTRAINT 'FK_TreeEventFragilityCurveElementEntity_TreeEventEntity' FOREIGN KEY ('TreeEventId') REFERENCES 'TreeEventEntity' ('TreeEventId') ON DELETE Cascade ON UPDATE Cascade
)
;

/* Create Indexes and Triggers */

CREATE INDEX 'IXFK_HydraulicConditionElementEntity_FragilityCurveElementEntity'
 ON 'HydraulicConditionElementEntity' ('FragilityCurveElementId' ASC)
;

CREATE INDEX 'IXFK_HydraulicConditionElementEntity_ProjectEntity'
 ON 'HydraulicConditionElementEntity' ('ProjectId' ASC)
;

CREATE INDEX 'IXFK_EventTreeEntity_ProjectEntity'
 ON 'EventTreeEntity' ('ProjectId' ASC)
;

CREATE INDEX 'IXFK_EventTreeEntity_TreeEventEntity'
 ON 'EventTreeEntity' ('MainTreeEventId' ASC)
;

CREATE INDEX 'IXFK_ExpertClassEstimationEntity_ExpertEntity'
 ON 'ExpertClassEstimationEntity' ('ExpertId' ASC)
;

CREATE INDEX 'IXFK_ExpertClassEstimationEntity_TreeEventEntity'
 ON 'ExpertClassEstimationEntity' ('TreeEventId' ASC)
;

CREATE INDEX 'IXFK_ExpertEntity_PersonEntity'
 ON 'ExpertEntity' ('PersonId' ASC)
;

CREATE INDEX 'IXFK_ExpertEntity_ProjectEntity'
 ON 'ExpertEntity' ('ProjectId' ASC)
;

CREATE INDEX 'IXFK_ProjectEntity_PersonEntity'
 ON 'ProjectEntity' ('ProjectLeaderId' ASC)
;

CREATE INDEX 'IXFK_TreeEventEntity_TreeEventEntity'
 ON 'TreeEventEntity' ('FailingEventId' ASC)
;

CREATE INDEX 'IXFK_TreeEventEntity_TreeEventEntity_02'
 ON 'TreeEventEntity' ('PassingEventId' ASC)
;

CREATE INDEX 'IXFK_TreeEventFragilityCurveElementEntity_FragilityCurveElementEntity'
 ON 'TreeEventFragilityCurveElementEntity' ('FragilityCurveElementId' ASC)
;

CREATE INDEX 'IXFK_TreeEventFragilityCurveElementEntity_TreeEventEntity'
 ON 'TreeEventFragilityCurveElementEntity' ('TreeEventId' ASC)
;
