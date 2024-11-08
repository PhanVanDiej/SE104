CREATE DATABASE STUDENT_MANAGEMENT
USE STUDENT_MANAGEMENT
SET DATEFORMAT dmy

CREATE TABLE REGULATION
(
	SchoolYear SMALLINT PRIMARY KEY,
	MinAge TINYINT NOT NULL,
	MaxAge TINYINT NOT NULL,
	MaxClassSize TINYINT NOT NULL,
	PassingGPA DECIMAL(5,2) NOT NULL,
	PassingGPAPerSubject DECIMAL(5,2) NOT NULL
)

CREATE TABLE USERS
(	ID VARCHAR(10) NOT NULL,
	Pass VARCHAR(64) NOT NULL,
	FullName NVARCHAR(50) NOT NULL,
	Email VARCHAR(50) UNIQUE NOT NULL,
	Access NVARCHAR(50) NOT NULL,
	Code VARCHAR(6),
	CONSTRAINT PK_USER PRIMARY KEY (ID)
)

CREATE TABLE STUDENT
(	ID VARCHAR(10),
	FullName NVARCHAR(50) NOT NULL,
	Gender NVARCHAR(3) NOT NULL,
	DateOfBirth DATE NOT NULL,
	Province NVARCHAR(30) NOT NULL,
	District NVARCHAR(30) NOT NULL,
	Commune NVARCHAR(30) NOT NULL,
	AddictiveAddress NVARCHAR(50),
	Email VARCHAR(50) UNIQUE NOT NULL,
	CONSTRAINT PK_STUDENT PRIMARY KEY (ID)
)

CREATE TABLE SUBJECTS
(	ID VARCHAR(10),
	SubjectName NVARCHAR(50) NOT NULL,
	CONSTRAINT PK_SUBJECT PRIMARY KEY (ID)
)

CREATE TABLE CLASS
(
	ID VARCHAR(10),
	ClassName NVARCHAR(10),
	SchoolYear SMALLINT,
	CONSTRAINT PK_CLASS PRIMARY KEY (ID)
)

CREATE TABLE TEACHING
(	TeacherID VARCHAR(10),
	ClassID VARCHAR(10),
	SubjectID VARCHAR(10),
	Term TINYINT,
	CONSTRAINT PK_TEACHING PRIMARY KEY (TeacherID, ClassID, SubjectID, Term)
)

CREATE TABLE LEARNING
(	StudentID VARCHAR(10),
	ClassID VARCHAR(10),
	Term TINYINT,
	GPA DECIMAL(5,2),
	IsPass NVarchar(10),
	Note NVARCHAR(20),
	CONSTRAINT PK_LEARNING PRIMARY KEY (StudentID, ClassID, Term)
)

CREATE TABLE SCORE
(	ClassID VARCHAR(10),
	SubjectID VARCHAR(10),
	StudentID VARCHAR(10),
	Term TINYINT,
	MiniTest DECIMAL(5,2),
	MidTermTest DECIMAL(5,2),
	FinalTermTest DECIMAL(5,2),
	Average DECIMAL(5,2),
	IsPass NVarchar(10),
	CONSTRAINT PK_SCORE PRIMARY KEY (ClassID, SubjectID, StudentID, Term)
)
CREATE TABLE ADDRESSES
(	Province NVARCHAR(30),
	District NVARCHAR(30),
	Commune NVARCHAR(30),
	CONSTRAINT PK_ADDRESS PRIMARY KEY (Province, District, Commune)
)

alter table teaching add constraint FK_TEACHERID FOREIGN KEY (TEACHERID) REFERENCES USERS(ID)
alter table teaching add constraint FK_CLASSID FOREIGN KEY (CLASSID) REFERENCES CLASS(ID)
alter table teaching add constraint FK_SUBJECTID FOREIGN KEY (SUBJECTID) REFERENCES SUBJECTS(ID)

ALTER TABLE LEARNING ADD CONSTRAINT FK_CLASSID_2 FOREIGN KEY (CLASSID) REFERENCES CLASS(ID)
ALTER TABLE LEARNING ADD CONSTRAINT FK_STUDENTID FOREIGN KEY (STUDENTID) REFERENCES STUDENT(ID)

ALTER TABLE TEACHING ADD CONSTRAINT UQ_TEACHING UNIQUE (CLASSID, SUBJECTID, TERM)
ALTER TABLE LEARNING ADD CONSTRAINT UQ_LEARNING UNIQUE (CLASSID, STUDENTID, TERM)

ALTER TABLE SCORE ADD CONSTRAINT FK_SCORE_TEACHING FOREIGN KEY (CLASSID, SUBJECTID, TERM) REFERENCES TEACHING(CLASSID, SUBJECTID, TERM)
ALTER TABLE SCORE ADD CONSTRAINT FK_SCORE_LEARNING FOREIGN KEY (CLASSID, STUDENTID, TERM) REFERENCES LEARNING(CLASSID, STUDENTID, TERM)


ALTER TABLE CLASS ADD CONSTRAINT FK_SCHOOLYEAR FOREIGN KEY (SCHOOLYEAR) REFERENCES REGULATION(SCHOOLYEAR)

alter table student add constraint FK_Address FOREIGN KEY (Province, District, Commune) references addresses(Province, District, Commune)

INSERT INTO REGULATION (SchoolYear, MinAge, MaxAge, MaxClassSize, PassingGPA, PassingGPAPerSubject)
VALUES
(2023, 15, 20, 45, 5.00, 5.00),
(2024, 15, 20, 45, 5.00, 5.00),
(2025, 15, 20, 45, 5.00, 5.00);
INSERT INTO CLASS (ID, ClassName, SchoolYear)
VALUES
('C001', 'Class A', 2023),
('C002', 'Class B', 2023),
('C003', 'Class C', 2023),
('C004', 'Class D', 2024);
INSERT INTO ADDRESSES (Province, District, Commune) VALUES 
(N'Thành phố Hồ Chí Minh', N'Quận 1', N'Phường Bến Nghé'),
(N'Thành phố Hồ Chí Minh', N'Quận 1', N'Phường Bến Thành'),
(N'Thành phố Hồ Chí Minh', N'Quận 3', N'Phường Võ Thị Sáu'),
(N'Thành phố Hồ Chí Minh', N'Quận 3', N'Phường 9'),
(N'Thành phố Hồ Chí Minh', N'Quận 5', N'Phường 1'),
(N'Thành phố Hồ Chí Minh', N'Quận 5', N'Phường 2'),
(N'Thành phố Hồ Chí Minh', N'Quận Bình Thạnh', N'Phường 19'),
(N'Thành phố Hồ Chí Minh', N'Quận Bình Thạnh', N'Phường 24'),
(N'Thành phố Hồ Chí Minh', N'Quận Gò Vấp', N'Phường 1'),
(N'Thành phố Hồ Chí Minh', N'Quận Gò Vấp', N'Phường 14'),
(N'Thành phố Hồ Chí Minh', N'Quận 7', N'Phường Tân Phong'),
(N'Thành phố Hồ Chí Minh', N'Quận 7', N'Phường Tân Hưng'),
(N'Thành phố Hồ Chí Minh', N'Quận 10', N'Phường 1'),
(N'Thành phố Hồ Chí Minh', N'Quận 10', N'Phường 15'),
(N'Thành phố Hồ Chí Minh', N'Quận 12', N'Phường Tân Chánh Hiệp'),
(N'Thành phố Hồ Chí Minh', N'Quận 12', N'Phường Hiệp Thành'),
(N'Thành phố Hồ Chí Minh', N'Quận Tân Bình', N'Phường 4'),
(N'Thành phố Hồ Chí Minh', N'Quận Tân Bình', N'Phường 10'),
(N'Thành phố Hồ Chí Minh', N'Thành phố Thủ Đức', N'Phường Linh Tây'),
(N'Thành phố Hồ Chí Minh', N'Thành phố Thủ Đức', N'Phường Bình Thọ');
INSERT INTO STUDENT (ID, FullName, Gender, DateOfBirth, Province, District, Commune, AddictiveAddress, Email)
VALUES
('S001', N'Nguyễn Văn A', N'M', '2005-03-15', N'Thành phố Hồ Chí Minh', N'Quận 1', N'Phường Bến Nghé', N'', 'nguyenvana@example.com'),
('S002', N'Nguyễn Thị B', N'F', '2006-07-20', N'Thành phố Hồ Chí Minh', N'Quận 3', N'Phường Võ Thị Sáu', N'', 'nguyenthib@example.com'),
('S003', N'Trần Văn C', N'M', '2007-01-10', N'Thành phố Hồ Chí Minh', N'Quận 5', N'Phường 2', N'', 'tranvanc@example.com'),
('S004', N'Lê Thị D', N'F', '2008-09-05', N'Thành phố Hồ Chí Minh', N'Quận Bình Thạnh', N'Phường 24', N'', 'lethid@example.com');

INSERT INTO SUBJECTS (ID, SubjectName)
VALUES
('SUB001', N'Mathematics'),
('SUB002', N'Physics'),
('SUB003', N'Chemistry'),
('SUB004', N'Biology');
select * from USERS
INSERT INTO USERS (ID, FullName, Pass, Email, Access, Code) VALUES 
('U001', N'Nguyễn Thị A', 'AfrQNK/RcPcN7HrVB5T9/Vp7fDzFZP2V4AMXCEBW12EPeujdWQV2H2E9nsTPw6WU', 'nguyenthia@example.com', N'Quản trị viên', NULL),
('U002', N'Trần Văn B', 'c7H7cyN6nS9EtXP3l+iu9CHqP4hkefrZzKrQW8TbrsSPzLm7JL9sh0oOTieWcxQm', 'tranvanb@example.com', N'Giáo viên', NULL),
('U003', N'Phạm Thị C','35DB71M15InzNpD2ocklHRaBOsY0x+i73eZnNkpoJsvmQFDVVrlmBYnXYHVv1N6h', 'phamthic@example.com', N'Giáo vụ', NULL),
('U004', N'Lê Văn D','P8x0FUxrn2Mr+H6mejqUR9y9lfkkl/Uh/N3dR55JXm1OXiNoNuEwVVa46RHAtHZc', 'levand@example.com', N'Phó hiệu trưởng chuyên môn', NULL);
INSERT INTO TEACHING (TeacherID, ClassID, SubjectID, Term)
VALUES
('U002', 'C001', 'SUB001', 1),
('U002', 'C002', 'SUB002', 1),
('U002', 'C003', 'SUB003', 1);

INSERT INTO LEARNING (StudentID, ClassID, Term)
VALUES
('S001', 'C001', 1),
('S001', 'C002', 1),
('S002', 'C003', 1),
('S003', 'C001', 1);


CREATE OR ALTER TRIGGER trg_AfterInsertTeaching
ON TEACHING
FOR INSERT
AS
BEGIN
    -- Insert empty score records for the newly inserted teaching assignments
    INSERT INTO SCORE (ClassID, SubjectID, StudentID, Term, MiniTest, MidTermTest, FinalTermTest, Average)
    SELECT i.ClassID, i.SubjectID, l.StudentID, i.Term, 0, 0, 0, 0
    FROM inserted i
    JOIN LEARNING l ON i.ClassID = l.ClassID AND i.Term = l.Term;
END;

CREATE OR ALTER TRIGGER trg_AfterDeleteTeaching
ON TEACHING
FOR DELETE
AS
BEGIN
    DELETE FROM SCORE
    WHERE EXISTS (
        SELECT 1
        FROM deleted d
        WHERE SCORE.ClassID = d.ClassID
          AND SCORE.SubjectID = d.SubjectID
          AND SCORE.Term = d.Term
    );
END;

-- Trigger to handle INSERT on LEARNING table
CREATE OR ALTER TRIGGER trg_AfterInsertLearning
ON LEARNING
FOR INSERT
AS
BEGIN
    INSERT INTO SCORE (ClassID, SubjectID, StudentID, Term, MiniTest, MidTermTest, FinalTermTest, Average)
    SELECT t.ClassID, t.SubjectID, i.StudentID, t.Term, 0, 0, 0, 0
    FROM TEACHING t
    JOIN inserted i ON t.ClassID = i.ClassID AND t.Term = i.Term;
END;

-- Trigger to handle DELETE on LEARNING table
CREATE TRIGGER trg_AfterDeleteLearning
ON LEARNING
FOR DELETE
AS
BEGIN
    DELETE FROM SCORE
    WHERE EXISTS (
        SELECT 1
        FROM deleted d
        WHERE SCORE.ClassID = d.ClassID
          AND SCORE.StudentID = d.StudentID
          AND SCORE.Term = d.Term
    );
END;

CREATE OR ALTER TRIGGER auto_calc_gpa_and_pass_trigger
ON SCORE
INSTEAD OF UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    -- Store the updated values in a temporary table
    SELECT *
    INTO #TempUpdatedScores
    FROM inserted;

    -- Calculate the Average column
    UPDATE #TempUpdatedScores
    SET Average = (MiniTest / 6.0 + MidTermTest * 2 / 6.0 + FinalTermTest * 3 / 6.0);

    -- Update the SCORE table with new values from the temporary table
    UPDATE s
    SET s.MiniTest = t.MiniTest,
        s.MidTermTest = t.MidTermTest,
        s.FinalTermTest = t.FinalTermTest,
        s.Average = t.Average
    FROM SCORE s
    INNER JOIN #TempUpdatedScores t
        ON s.StudentID = t.StudentID
        AND s.ClassID = t.ClassID
        AND s.Term = t.Term;

    -- Update the IsPass column in the SCORE table
    UPDATE s
    SET IsPass = CASE
                     WHEN s.Average >= (
                            SELECT TOP 1 r.PassingGPAPerSubject
                            FROM REGULATION r
                            JOIN CLASS c ON c.SchoolYear = r.SchoolYear
                            JOIN LEARNING l ON l.ClassID = c.ID
                            WHERE l.ClassID = s.ClassID
                          )
                     THEN N'Đạt'
                     ELSE N'Không đạt'
                 END
    FROM SCORE s

    -- Update GPA in the LEARNING table based on the updated SCORE data
    UPDATE l
    SET GPA = (
            SELECT AVG(s.Average)
            FROM SCORE s
            WHERE s.StudentID = l.StudentID
                AND s.ClassID = l.ClassID
                AND s.Term = l.Term
        )
    FROM LEARNING l
    WHERE EXISTS (
        SELECT 1
        FROM #TempUpdatedScores t
        WHERE t.StudentID = l.StudentID
          AND t.ClassID = l.ClassID
          AND t.Term = l.Term
    );

    -- Update IsPass in the LEARNING table based on the new GPA
    UPDATE l
    SET IsPass = CASE
                     WHEN l.GPA >= (
                            SELECT TOP 1 r.PassingGPA
                            FROM REGULATION r
                            JOIN CLASS c ON c.SchoolYear = r.SchoolYear
                            WHERE c.ID = l.ClassID
                          )
                     THEN N'Đạt'
                     ELSE N'Không đạt'
                 END
    FROM LEARNING l
    WHERE EXISTS (
        SELECT 1
        FROM #TempUpdatedScores t
        WHERE t.StudentID = l.StudentID
          AND t.ClassID = l.ClassID
          AND t.Term = l.Term
    );

    DROP TABLE #TempUpdatedScores;
END;


UPDATE SCORE
SET MiniTest = 7.0, MidTermTest = 8.0, FinalTermTest = 9.0
WHERE ClassID = 'C001' AND StudentID = 'S001' AND Term = 1;
select * from score
select * from LEARNING
UPDATE SCORE
SET MiniTest = 6.0, MidTermTest = 7.0, FinalTermTest = 8.5
WHERE ClassID = 'C002' AND StudentID = 'S001' AND Term = 1;

UPDATE SCORE
SET MiniTest = 7.0, MidTermTest = 6.5, FinalTermTest = 7.5
WHERE ClassID = 'C003' AND StudentID = 'S002' AND Term = 1;

UPDATE SCORE
SET MiniTest = 8.0, MidTermTest = 9.0, FinalTermTest = 8.5
WHERE ClassID = 'C001' AND StudentID = 'S003' AND Term = 1;
UPDATE SCORE
SET MiniTest = 9.0, MidTermTest = 8.5, FinalTermTest = 9.5
WHERE ClassID = 'C001' AND SubjectID = 'SUB001' AND StudentID = 'S001' AND Term = 1;
CREATE OR ALTER TRIGGER delete_related_scores
ON LEARNING
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete related rows from the SCORE table
    DELETE s
    FROM SCORE s
    INNER JOIN deleted d
        ON s.StudentID = d.StudentID
        AND s.ClassID = d.ClassID
        AND s.Term = d.Term;

    -- Delete the rows from the LEARNING table
    DELETE l
    FROM LEARNING l
    INNER JOIN deleted d
        ON l.StudentID = d.StudentID
        AND l.ClassID = d.ClassID
        AND l.Term = d.Term;
END;
CREATE OR ALTER TRIGGER delete_related_scores_2
ON TEACHING
INSTEAD OF DELETE
AS
BEGIN
    SET NOCOUNT ON;

    -- Delete related rows from the SCORE table
    DELETE s
    FROM SCORE s
    INNER JOIN deleted d
        ON s.SubjectID = d.SubjectID
        AND s.ClassID = d.ClassID
        AND s.Term = d.Term;

    -- Delete the rows from the LEARNING table
    DELETE t
    FROM TEACHING t
    INNER JOIN deleted d
        ON t.SubjectID = d.SubjectID
        AND t.ClassID = d.ClassID
        AND t.Term = d.Term;
END;
CREATE OR ALTER TRIGGER trg_CheckMaxClassSize
ON LEARNING
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @ClassID VARCHAR(10);
    DECLARE @Term TINYINT;
    DECLARE @MaxClassSize TINYINT;
    DECLARE @CurrentStudentCount INT;

    -- Get the ClassID and Term from inserted records
    SELECT @ClassID = ClassID, @Term = Term
    FROM inserted;

    -- Get the SchoolYear of the class
    DECLARE @SchoolYear SMALLINT;
    SELECT @SchoolYear = Class.SchoolYear
    FROM CLASS
    WHERE ID = @ClassID;

    -- Get the MaxClassSize for the corresponding SchoolYear from REGULATION table
    SELECT @MaxClassSize = MaxClassSize
    FROM REGULATION
    WHERE SchoolYear = @SchoolYear;

    -- Count current number of students in the class and term
    SELECT @CurrentStudentCount = COUNT(*)
    FROM LEARNING
    WHERE ClassID = @ClassID AND Term = @Term;

    -- Check if current student count exceeds MaxClassSize
    IF @CurrentStudentCount >= @MaxClassSize
    BEGIN
        RAISERROR('Number of students exceeds MaxClassSize (%d) for SchoolYear %d', 16, 1, @MaxClassSize, @SchoolYear) WITH NOWAIT;
        ROLLBACK TRANSACTION; -- Rollback the transaction
    END;
END;
CREATE OR ALTER TRIGGER trg_CheckStudentAge
ON STUDENT
AFTER INSERT, UPDATE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @MinAge TINYINT;
    DECLARE @MaxAge TINYINT;
    DECLARE @StudentAge TINYINT;

    -- Fetch MinAge and MaxAge from REGULATION table
    SELECT @MinAge = MinAge, @MaxAge = MaxAge
    FROM REGULATION
    WHERE SchoolYear = (SELECT MAX(SchoolYear) FROM REGULATION); -- Adjust to fetch the correct regulation

    -- Check age for each inserted/updated student
    IF EXISTS (
        SELECT 1
        FROM inserted i
        WHERE DATEDIFF(YEAR, i.DateOfBirth, GETDATE()) < @MinAge
           OR DATEDIFF(YEAR, i.DateOfBirth, GETDATE()) > @MaxAge
    )
    BEGIN
        RAISERROR('Student age does not fit MinAge (%d) and MaxAge (%d)', 16, 1, @MinAge, @MaxAge) WITH NOWAIT;
        ROLLBACK TRANSACTION; -- Rollback the transaction
    END;
END;


INSERT INTO TEACHING (TeacherID, ClassID, SubjectID, Term)
VALUES
('U002', 'C004', 'SUB004', 2)

INSERT INTO LEARNING (StudentID, ClassID, Term)
VALUES
('S005', 'C004', 2)
delete LEARNING where StudentID ='s005'
delete TEACHING where SubjectID ='sub004'
