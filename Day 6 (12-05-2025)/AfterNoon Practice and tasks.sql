--Transaction Examples Practice

DROP TABLE IF EXISTS table_Accounts;

CREATE TABLE table_Accounts (
    account_id SERIAL PRIMARY KEY,
    account_name VARCHAR(100),
    balance NUMERIC(10, 2)
);

INSERT INTO table_Accounts (account_name, balance)
VALUES
('Alice', 5000.00),
('Bob', 3000.00);

SELECT * FROM table_Accounts;

-- SIMPLE TRANSACTION EXAMPLE

BEGIN;

UPDATE table_Accounts
SET balance = balance - 1000
WHERE account_name = 'Alice';

UPDATE table_Accounts
SET balance = balance + 1000
WHERE account_name = 'Bob';

COMMIT;

SELECT * FROM table_Accounts;


-- ROLL BACK WITH AN ERROR

BEGIN;

UPDATE table_Accounts
SET balance = balance + 1000
WHERE account_name = 'Alice';

-- Typo in table name
UPDATE tbl_bank_account
SET balance = balance - 1000
WHERE account_name = 'Bob';

ROLLBACK;

SELECT * FROM table_Accounts;

-- SAVEPOINT AND PARTIAL ROLLBACK

BEGIN;

UPDATE table_Accounts
SET balance = balance - 1000
WHERE account_name = 'Alice';

SAVEPOINT sp_after_alice;

-- typo to simulate error
UPDATE tbl_bank_account
SET balance = balance + 1000
WHERE account_name = 'Bob';

ROLLBACK TO sp_after_alice;

-- Correct the update
UPDATE table_Accounts
SET balance = balance + 1000
WHERE account_name = 'Bob';

COMMIT;

SELECT * FROM table_Accounts;

-- WITHOUT COMMIT

-- session 1
BEGIN;

UPDATE table_Accounts
SET balance = balance + 1000
WHERE account_name = 'Alice';

-- Didn't commited yet!

-- session 2
SELECT * FROM table_Accounts;

-- session 1
COMMIT;

-- NOW IN SESSION 2 UPDATED;

-- CONDITION BASED TRANSFER

BEGIN;

DO $$
DECLARE
    var_balance NUMERIC;
BEGIN
    SELECT balance INTO var_balance
    FROM table_Accounts
    WHERE account_name = 'Bob';

    IF var_balance >= 7500 THEN
        UPDATE table_Accounts SET balance = balance + 1500 WHERE account_name = 'Alice';
        UPDATE table_Accounts SET balance = balance - 1500 WHERE account_name = 'Bob';
    ELSE
        RAISE NOTICE 'Insufficient Funds!';
        ROLLBACK;
    END IF;
END;
$$;

SELECT * FROM table_Accounts;


-- MVCC

-- SAME AS WITHOUT COMMIT DONE AND SEEN THE DIFFERENCE IN DIFF SESSIONS;

-- LOST UPDATE SIMULATION

-- SESSION 1

BEGIN;
SELECT balance FROM table_Accounts WHERE account_name = 'Alice';  -- 4000

-- SESSION 2

BEGIN;
UPDATE table_Accounts SET balance = 7000 WHERE account_name = 'Alice';
COMMIT;

-- SESSION 1

UPDATE table_Accounts SET balance = 3000 WHERE account_name = 'Alice';
COMMIT;

-- NOW THE SESSION 2 UPDATE IS LOST.. SESSION 1'S UPDATE HAS OVER WRITEN ON THAT UPDATE;

-- PESSIMISTIC LOCKING

BEGIN;
SELECT * FROM table_Accounts
WHERE account_name = 'Alice'
FOR UPDATE;

UPDATE table_Accounts
SET balance = balance + 200
WHERE account_name = 'Alice';
COMMIT;

-- WHEN WE TRY TO UPDATE ON THAT ROW IN OTHER SESSIONS, 
-- IT WAITS FOR THE LOCK TO RELEASE TO COMPLETE THE ACTION;

-- serialisable --> phantom read block

-- session 1
BEGIN ISOLATION LEVEL SERIALIZABLE;
SELECT COUNT(*) FROM table_Accounts WHERE account_name = 'Bob';

-- session 2
BEGIN;
INSERT INTO tbl_users (name) VALUES ('Bob');
COMMIT;

--session 1
SELECT COUNT(*) FROM table_Accounts WHERE account_name = 'Bob';
COMMIT;


/*

Question:
In a transaction, if I perform multiple updates and an error happens in the third statement, but I have not used SAVEPOINT, what will happen if I issue a ROLLBACK?
Will my first two updates persist?

No, none of the changes will persist.
A ROLLBACK undoes the entire transaction, not just the failed part.
Without SAVEPOINT, you can't do a partial rollback.

Question:
Suppose Transaction A updates Alice’s balance but does not commit. Can Transaction B read the new balance if the isolation level is set to READ COMMITTED?

No, READ COMMITTED will only let Transaction B see committed data.
Since Transaction A hasn't committed, B will see the old balance.

Question:
What will happen if two concurrent transactions both execute:
UPDATE table_Accounts SET balance = balance - 100 WHERE account_name = 'Alice';
at the same time? Will one overwrite the other?

PostgreSQL uses row-level locks under the hood.
The second transaction will wait for the first one to commit or rollback.
So they won’t overwrite each other, but execute serially to maintain consistency.

Question:
If I issue ROLLBACK TO SAVEPOINT after_alice;, will it only undo changes made after the savepoint or everything?

Only changes made after the savepoint will be undone.
Changes before the savepoint remain intact within the transaction.

Question:
Which isolation level in PostgreSQL prevents phantom reads?

SERIALIZABLE. It's the highest isolation level and prevents phantom reads, 
repeatable read anomalies, and ensures complete consistency.

Question:
Can Postgres perform a dirty read (reading uncommitted data from another transaction)?

No. PostgreSQL’s MVCC architecture prevents dirty reads, even at its lowest level (READ COMMITTED).
Uncommitted changes are never visible to other transactions.

Question:
If autocommit is ON (default in Postgres), and I execute an UPDATE, is it safe to assume the change is immediately committed?

No. PostgreSQL’s MVCC architecture prevents dirty reads, even at its lowest level (READ COMMITTED).
Uncommitted changes are never visible to other transactions.

Question:
If I do this:

BEGIN;
UPDATE accounts SET balance = balance - 500 WHERE id = 1;
-- (No COMMIT yet)
And from another session, I run:

SELECT balance FROM accounts WHERE id = 1;
Will the second session see the deducted balance?

No. The second session will not see the uncommitted change.
Thanks to MVCC, it will read the last committed version of the row.

*/







