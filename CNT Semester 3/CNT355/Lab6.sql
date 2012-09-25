--q1
use l6

IF EXISTS (SELECT name FROM sysobjects
           WHERE name = 'my_stores')
Drop TABLE my_stores
GO

select * into my_stores
from pubs.dbo.stores 
where 1 = 0

--q2
use pubs

select max (stor_id) from stores

--q3
insert my_stores
values ('8043', 'Cartman''s Boutique',
		'666 Misery Lane', 'Southpark', 'CA', '90210')

--q4
insert my_stores
values ('8044', 'Brink of death', null, null, null, null)

--q5
insert my_stores (zip, state, city, stor_address, stor_name, stor_id)
values ('98034', 'WA', 'Remulade',
		''

--q6
insert into my_stores
	select * from pubs..stores
	where state like 'WA'

--q7
insert my_stores
	select *
	from pubs..stores
	where state like 'WA'

--q8
insert my_stores
	select distinct st.stor_id, stor_name,
		stor_address, city, state, zip
	from pubs..stores st
		join pubs.sales

--q9
delete my_titles
where type not in ('popular_comp', 'business')

--q10
delete my_titles
where price > 20

--q11
delete my_titles
where title_id in
(
	select title_id
	from my_sales
	where ord_date='Sep 14, 1994'
)

--q12
update my_titles
set price = round (price * 1.1, 2)

--q13
update my_titles
set qty = qty+5
where ord_date >= 'Jan 1, 1994'