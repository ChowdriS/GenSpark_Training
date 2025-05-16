-- Phase 1 : 
-- You are tasked with building a PostgreSQL-backed database for an EdTech company that manages online training and certification programs for individuals across various technologies.
-- The goal is to:
-- Design a normalized schema
-- Support querying of training data
-- Ensure secure access
-- Maintain data integrity and control over transactional updates
-- Database planning (Nomalized till 3NF)
-- A student can enroll in multiple courses
-- Each course is led by one trainer
-- Students can receive a certificate after passing
-- Each certificate has a unique serial number
-- Trainers may teach multiple courses


-- status_manager 
-- -> id int, status_message text

-- student
-- -> student_id int, student_name text, standard_id int

-- standard_master
-- -> standard_id int, class int, section char, year int

-- trainer
-- -> trainer_id int, trainer_name text

-- course
-- -> course_id int, course_name text, course_category_id int, course_duration 

-- category_master
-- -> category_id int, category_name text

-- batch_master
-- -> batch_id int, trainer_id, course_id, start_date date, end_date date

-- course_enrollment
-- -> course_enrollment_id int, student_id int , batch_id int, payment_id int

-- certificate
-- -> certificate_id int, course_enrollment_id int, certificate_serial text, issue_date date, status_id int

-- course_result
-- -> result_id int, course_enrollment_id int, passed boolean

-- payment_master
-- -> payment_id int, payment_amt int, payment_type_id int, payment_status_id text

-- type_master
-- -> type_id int, type text

------------------------------------------------------------------------------------

-- Tables to Design (Normalized to 3NF):
-- 1. **students**
--    * `student_id (PK)`, `name`, `email`, `phone`
-- 2. **courses**
--    * `course_id (PK)`, `course_name`, `category`, `duration_days`
-- 3. **trainers**
--    * `trainer_id (PK)`, `trainer_name`, `expertise`
-- 4. **enrollments**
--    * `enrollment_id (PK)`, `student_id (FK)`, `course_id (FK)`, `enroll_date`
-- 5. **certificates**
--    * `certificate_id (PK)`, `enrollment_id (FK)`, `issue_date`, `serial_no`
-- 6. **course\_trainers** (Many-to-Many if needed)
--    * `course_id`, `trainer_id`

-- Phase 2: DDL & DML
-- * Create all tables with appropriate constraints (PK, FK, UNIQUE, NOT NULL)
-- * Insert sample data using `INSERT` statements
-- * Create indexes on `student_id`, `email`, and `course_id`

create table students (student_id serial primary key, name text, 
    email text unique not null, phone text);

create table courses (course_id serial primary key, course_name text, 
    category text, duration_days int);

create table trainers (trainer_id serial primary key, trainer_name text not null, 
    expertise text);

create table enrollments (enrollment_id serial primary key, student_id int, 
    course_id int, enroll_date date, foreign key (student_id) references students(student_id), 
    foreign key (course_id) references courses(course_id));

create table certificates (certificate_id serial primary key, enrollment_id int, issue_date date, 
    serial_no text unique, foreign key (enrollment_id) references enrollments(enrollment_id));

create table course_trainers (course_id int, trainer_id int, primary key(course_id, trainer_id), 
    foreign key (course_id) references courses(course_id), 
    foreign key (trainer_id) references trainers(trainer_id));

insert into students (name, email, phone) values 
('student1', 'student1@example.com', '1234567890'),
('student2', 'student2@example.com', '2345678901'),
('student3', 'student3@example.com', '3456789012');

insert into courses (course_name, category, duration_days) values 
('python basics', 'programming', 30),
('data science', 'analytics', 45),
('web development', 'development', 40);

insert into trainers (trainer_name, expertise) values 
('trainer1', 'python'),
('trainer2', 'data science'),
('trainer3', 'web development');

insert into enrollments (student_id, course_id, enroll_date) values 
(1, 1, '2025-01-15'),
(2, 2, '2025-02-01'),
(3, 3, '2025-03-10');

insert into certificates (enrollment_id, issue_date, serial_no) values 
(1, '2025-02-15', 'CERT001'),
(2, '2025-03-01', 'CERT002'),
(3, '2025-04-10', 'CERT003');

insert into course_trainers (course_id, trainer_id) values 
(1, 1),
(2, 2),
(3, 3);

create index idx_students_student_id on students(student_id);
create index idx_students_email on students(email);
create index idx_courses_course_id on courses(course_id);

select * from students;
select * from enrollments;

-- Phase 3: SQL Joins Practice
-- Write queries to:
-- 1. List students and the courses they enrolled in
-- 2. Show students who received certificates with trainer names
-- 3. Count number of students per course

select s.name, c.course_name from enrollments e
join courses c on e.course_id = c.course_id 
join students s on s.student_id = e.student_id;

select s.name, t.trainer_name from certificates ce
join enrollments e on e.enrollment_id = ce.enrollment_id
join course_trainers ct on e.course_id = ct.course_id 
join trainers t on ct.trainer_id = t.trainer_id
join students s on s.student_id = e.student_id;

select c.course_name, count(e.student_id) as students_count 
from enrollments e
join courses c on e.course_id = c.course_id
group by c.course_name 
order by students_count, c.course_name;


-- Phase 4: Functions & Stored Procedures
-- Function:
-- Create `get_certified_students(course_id INT)`
-- → Returns a list of students who completed the given course and received certificates.
-- Stored Procedure:
-- Create `sp_enroll_student(p_student_id, p_course_id)`
-- → Inserts into `enrollments` and conditionally adds a certificate if completed (simulate with status flag).

create or replace function get_certified_students(p_course_id INT)
returns table(student_id int, student_name text) as $$
begin
	return query
	select s.student_id, s.name from certificates c 
	join enrollments e on e.enrollment_id = c.enrollment_id
	join students s on s.student_id = e.student_id
	where e.course_id = p_course_id;
end; $$ language plpgsql;

select * from get_certified_students(2);

create or replace procedure sp_enroll_student(p_student_id int, p_course_id int, status_flag text) as $$
declare v_enroll_id int;
begin
	insert into enrollments (student_id, course_id, enroll_date) values 
	(p_student_id, p_course_id, current_date) returning enrollment_id into v_enroll_id;
	if(status_flag = 'completed') then
		insert into certificates (enrollment_id, issue_date, serial_no)
        values (v_enroll_id, current_date, 'CERT_' || v_enroll_id);
	end if;
end; $$ language plpgsql;

call sp_enroll_student(2,3,'Notcompleted');

select * from enrollments;
select * from certificates;

-- Phase 5: Cursor
-- Use a cursor to:
-- * Loop through all students in a course
-- * Print name and email of those who do not yet have certificates

create or replace procedure sp_GetCourseNotCompltedStudents() as $$
declare student_rec record;
	status_flag int;
	student_cur cursor for
		select e.enrollment_id, s.student_id, s.name, e.course_id from enrollments e 
		join students s on e.student_id = s.student_id order by s.student_id;
begin
	open student_cur;
	fetch student_cur into student_rec;
	while found loop
		select count(certificate_id) into status_flag from certificates where student_rec.enrollment_id = certificates.enrollment_id;
		if(status_flag = 0) then
			raise notice 'StudentName - %, courseId - % => Not Completed', student_rec.name,student_rec.course_id;
		end if;
		fetch student_cur into student_rec;
	end loop;
	close student_cur;
end; $$ language plpgsql;

call sp_GetCourseNotCompltedStudents();

-- Phase 6: Security & Roles
-- 1. Create a `readonly_user` role:
--    * Can run `SELECT` on `students`, `courses`, and `certificates`
--    * Cannot `INSERT`, `UPDATE`, or `DELETE`
-- 2. Create a `data_entry_user` role:
--    * Can `INSERT` into `students`, `enrollments`
--    * Cannot modify certificates directly

create role readonly_user with login password 'user_pass';

grant select on students to readonly_user;
grant select on courses to readonly_user;
grant select on certificates to readonly_user;

revoke insert, update, delete on students from readonly_user;
revoke insert, update, delete on courses from readonly_user;
revoke insert, update, delete on certificates from readonly_user;

create role data_entry_user with login password 'user_pass';

grant insert on students to data_entry_user;
grant insert on enrollments to data_entry_user;

revoke all on certificates from data_entry_user;


-- Phase 7: Transactions & Atomicity

-- Write a transaction block that:

-- * Enrolls a student
-- * Issues a certificate
-- * Fails if certificate generation fails (rollback)

-- ```sql
-- BEGIN;
-- -- insert into enrollments
-- -- insert into certificates
-- -- COMMIT or ROLLBACK on error
-- ```

create or replace procedure sp_enrollAndCertify(p_student_id int, p_course_id int) as $$
declare v_enroll_id int;
begin
	begin
        insert into enrollments (student_id, course_id, enroll_date) 
        values (p_student_id, p_course_id, current_date) 
        returning enrollment_id into v_enroll_id;
		
        insert into certificates (enrollment_id, issue_date, serial_no)
        values (v_enroll_id, current_date, 'CERT_' || v_enroll_id);
		
        raise notice 'Enrollment and certification successful for student %', p_student_id;
    exception when others then
		rollback;
		raise notice 'Error occurred. Transaction rolled back.';
    end;
end; $$ language plpgsql;

call sp_enrollAndCertify(1,3);

-- select * from enrollments;
-- select * from certificates;
-- delete from enrollments where enrollment_id = 9 or enrollment_id = 10;