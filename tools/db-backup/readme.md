# Production

## Certificate

TODO: The merging of two certificates is required as long as the baltimor certificate is valid. Somewhen in future,
this certificate will be revoked. As this happens, we no longer need it and we can just use the certificate from DigiCert.
See https://learn.microsoft.com/en-gb/azure/mariadb/concepts-certificate-rotation

For connections to the production database, two certificates are required. To fetch them, run:

```powershell
> Invoke-WebRequest https://www.digicert.com/CACerts/BaltimoreCyberTrustRoot.crt.pem -OutFile BaltimoreCyberTrustRoot.crt.pem
> Invoke-WebRequest https://cacerts.digicert.com/DigiCertGlobalRootG2.crt.pem -OutFile DigiCertGlobalRootG2.crt.pem
```

Create a new file, name it `Combined.crt.pem` and merge the content of the two downloaded certificates as below:

```
-----BEGIN CERTIFICATE-----
<content of BaltimoreCyberTrustRoot.crt.pem>
-----END CERTIFICATE-----
-----BEGIN CERTIFICATE-----
<content of DigiCertGlobalRootG2.crt.pem>
-----END CERTIFICATE-----
```

## Backup

```powershell
> mysqldump `
    -u theadmin `
    -p `
    --host=rolx-database.mariadb.database.azure.com `
    --ssl-ca=Combined.crt.pem `
    --single-transaction `
    --extended-insert `
    --result-file=rolx_production.sql `
    rolx_production
```

# Development

## Backup

```powershell
> mysqldump `
    -u root `
    -p `
    --single-transaction `
    --extended-insert `
    --result-file=db_rolx.sql `
    db_rolx
```

## Drop and recreate database

```powershell
> mysql -u root -p -e "drop database db_rolx; create database db_rolx character set utf8mb4 collate utf8mb4_unicode_ci;"
```

## Restore

When restoring dumps from production, they have to be sanitized before:

```powershell
> python sanitize4dev.py rolx_production.sql rolx_production_4_dev.sql
```


```powershell
> cmd /C "mysql -u root -p db_rolx < rolx_production_4_dev.sql"
```
