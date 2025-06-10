CREATE OR REPLACE FUNCTION get_paginated_tickets(page_number INT, page_size INT)
RETURNS TABLE(
    "Id" UUID,
    "TicketTypeId" UUID,
    "UserId" UUID,
    "EventId" UUID,
    "BookedQuantity" INT,
    "TotalPrice" NUMERIC,
    "BookedAt" TIMESTAMPTZ,
    "Status" INT,      
    "PaymentId" UUID       
) AS $$
BEGIN
    RETURN QUERY
    SELECT
        t."Id",
        t."TicketTypeId",
        t."UserId",
        t."EventId",
        t."BookedQuantity",
        t."TotalPrice",
        t."BookedAt",
        t."Status",            
        t."PaymentId"          
    FROM public."Tickets" t
    LIMIT page_size
    OFFSET (page_number - 1) * page_size;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION filter_events(search_element TEXT, event_date DATE)
RETURNS TABLE(
    "Id" UUID,
    "Title" VARCHAR(200),
    "Description" VARCHAR(200),  -- Change this to match the column's data type
    "EventDate" TIMESTAMPTZ,
    "EventType" INT,      -- Adjust according to the actual type of EventType
    "EventStatus" INT,    -- Adjust according to the actual type of EventStatus
    "IsDeleted" BOOLEAN,
    "CreatedAt" TIMESTAMPTZ,
    "UpdatedAt" TIMESTAMPTZ,
    "ManagerId" UUID
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        e."Id", 
        e."Title", 
        e."Description", 
        e."EventDate", 
        e."EventType", 
        e."EventStatus", 
        e."IsDeleted", 
        e."CreatedAt", 
        e."UpdatedAt", 
        e."ManagerId"
    FROM public."Events" e
    WHERE 
        e."IsDeleted" = false
        AND
        (
            search_element IS NULL
            OR LOWER(e."Description") LIKE LOWER('%' || search_element || '%')
        )
        AND
        (
            event_date IS NULL
            OR DATE(e."EventDate") = event_date
        );
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_paginated_events(page_number INT, page_size INT)
RETURNS TABLE(
    "Id" UUID,
    "Title" VARCHAR(200),
    "Description" VARCHAR(200),  -- Change this to match the column's data type
    "EventDate" TIMESTAMPTZ,
    "EventType" INT,      -- Adjust according to the actual type of EventType
    "EventStatus" INT,    -- Adjust according to the actual type of EventStatus
    "IsDeleted" BOOLEAN,
    "CreatedAt" TIMESTAMPTZ,
    "UpdatedAt" TIMESTAMPTZ,
    "ManagerId" UUID
) AS $$
BEGIN
    RETURN QUERY
    SELECT 
        e."Id", 
        e."Title", 
        e."Description", 
        e."EventDate", 
        e."EventType", 
        e."EventStatus", 
        e."IsDeleted", 
        e."CreatedAt", 
        e."UpdatedAt", 
        e."ManagerId"
    FROM public."Events" e
    WHERE e."IsDeleted" = false
    OFFSET (page_number - 1) * page_size
    LIMIT page_size;
END;
$$ LANGUAGE plpgsql;