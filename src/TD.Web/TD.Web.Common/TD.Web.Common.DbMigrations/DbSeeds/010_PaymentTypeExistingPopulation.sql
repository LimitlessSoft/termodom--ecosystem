-- Updates PaymentType table to set the first active payment type as default
with cte as (
    select "Id"
    from "PaymentTypes"
    where "IsActive" = true
    order by "Id"
    limit 1
    )
update "PaymentTypes"
set "IsDefault" = true
where "Id" = (select "Id" from cte);

-- Updates Users table to set DefaultPaymentTypeId to the first active payment type which is set as default
with cte as (
    select "Id"
    from "PaymentTypes"
    where "IsDefault" = true
    order by "Id"
    limit 1
    )
update "Users"
set "DefaultPaymentTypeId" = (select "Id" from cte);