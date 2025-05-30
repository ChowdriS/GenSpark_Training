-- Table Schema:	
-- Create Tables with Integrity Constrains:

-- a)	EMP (empno - primary key, empname, salary, deptname - references entries in a deptname of department table with null constraint, bossno - references entries in an empno of emp table with null constraint)
-- b)	DEPARTMENT (deptname - primary key, floor, phone, mgr_id - references entries in an empno of emp table not null)
-- c)	SALES (salesno - primary key, saleqty, itemname -references entries in a itemname of item table with not null constraint, deptname - references entries in a deptname of department table with not null constraint)
-- d)	ITEM (itemname - primary key, itemtype, itemcolor)


CREATE DATABASE EMS;

use EMS;

CREATE TABLE ITEM (
   itemname VARCHAR(50) PRIMARY KEY,
   itemtype VARCHAR(50),
   itemcolor VARCHAR(30)
);

CREATE TABLE DEPARTMENT (
   deptname VARCHAR(50) PRIMARY KEY,
   floor INT,
   phone VARCHAR(15),
   empno INT NOT NULL,
   FOREIGN KEY (empno) REFERENCES EMP(empno)
);

CREATE TABLE EMP (
   empno INT PRIMARY KEY,
   empname VARCHAR(100),
   salary DECIMAL(10, 2),
   deptname VARCHAR(50) NOT NULL,
   bossno INT,
   FOREIGN KEY (bossno) REFERENCES EMP(empno)
);

CREATE TABLE SALES (
   salesno INT PRIMARY KEY,
   saleqty INT,
   itemname VARCHAR(50) NOT NULL,
   deptname VARCHAR(50) NOT NULL,
   FOREIGN KEY (itemname) REFERENCES ITEM(itemname),
   FOREIGN KEY (deptname) REFERENCES DEPARTMENT(deptname)
);


BULK INSERT EMP
FROM 'C:\GenSpark_Training\Day 2 (06-05-2025)\csv\emp.csv'
WITH (
   FIELDTERMINATOR = ',',
   ROWTERMINATOR = '\n',     
   FIRSTROW = 2
);

BULK INSERT DEPARTMENT
FROM 'C:\GenSpark_Training\Day 2 (06-05-2025)\csv\dept.csv'
WITH (
   FIELDTERMINATOR = ',',
   ROWTERMINATOR = '\n',     
   FIRSTROW = 2
);

ALTER TABLE EMP
ADD FOREIGN KEY (deptname) REFERENCES DEPARTMENT(deptname);

BULK INSERT ITEM
FROM 'C:\GenSpark_Training\Day 2 (06-05-2025)\csv\item.csv'
WITH (
   FIELDTERMINATOR = ',',
   ROWTERMINATOR = '\n',     
   FIRSTROW = 2
);

BULK INSERT SALES
FROM 'C:\GenSpark_Training\Day 2 (06-05-2025)\csv\sales.csv'
WITH (
   FIELDTERMINATOR = ',',
   ROWTERMINATOR = '\n',     
   FIRSTROW = 2
);

SELECT * from EMP;
SELECT * from SALES;
SELECT * from ITEM;
SELECT * from DEPARTMENT;