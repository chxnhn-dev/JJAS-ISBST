-- JJAS-ISBST: Professional Session Tracking
-- Run this once on your JJAS-ISBST database.
-- This keeps login sessions out of tbl_User (IsActive remains account status).

IF OBJECT_ID('dbo.tbl_AppSession', 'U') IS NULL
BEGIN
    CREATE TABLE dbo.tbl_AppSession (
        SessionID UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
        PrincipalType NVARCHAR(20) NOT NULL,   -- 'admin' or 'user'
        PrincipalID INT NOT NULL,              -- AdminID or UserID
        Username NVARCHAR(100) NOT NULL,
        FullName NVARCHAR(200) NULL,
        Role NVARCHAR(50) NULL,

        CreatedAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        LastSeenAt DATETIME2 NOT NULL DEFAULT SYSDATETIME(),
        ExpiresAt DATETIME2 NOT NULL,
        EndedAt DATETIME2 NULL,
        EndReason NVARCHAR(50) NULL,

        MachineName NVARCHAR(128) NULL,
        AppVersion NVARCHAR(50) NULL
    );

    CREATE INDEX IX_tbl_AppSession_Principal_Active
        ON dbo.tbl_AppSession(PrincipalType, PrincipalID, EndedAt, ExpiresAt);
END
GO
