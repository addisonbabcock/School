use pubs

--q1
select		stores.stor_name 'Store name',
			sum (sales.qty) 'Books sold'
from		sales
	join	stores
	on stores.stor_id = sales.stor_id
group by	stores.stor_name--, stores.stor_id -- needed if store names are not unique

--q2
select		stores.stor_name 'Store name',
			sum (sales.qty) 'Books sold'
from		sales
	join	stores
	on stores.stor_id = sales.stor_id
where		year (sales.ord_date) in (1992, 1993)
group by	stores.stor_name--, stores.stor_id -- needed if store names are not unique

--q3
select		stores.stor_name 'Store name',
			sum (sales.qty) 'Books sold'
from		sales
	join	stores
	on stores.stor_id = sales.stor_id
where		year (sales.ord_date) in (1992, 1993)
group by	stores.stor_name, stores.stor_id -- needed if store names are not unique
having		avg (qty) >= 26

--q4
select		authors.au_lname 'Last name',
			sum (titles.ytd_sales * titles.price * titles.royalty / 100 - titles.advance) 'Money owed'
from		authors
	join	titleauthor
	on		authors.au_id = titleauthor.au_id
		join	titles
		on		titleauthor.title_id = titles.title_id
group by	authors.au_lname, authors.au_id --au_id needed because names are not unique

--q5
select		authors.au_lname 'Last name',
			sum (titles.ytd_sales * titles.price * titles.royalty / 100 - titles.advance) 'Money owed'
from		authors
	join	titleauthor
	on		authors.au_id = titleauthor.au_id
		join	titles
		on		titleauthor.title_id = titles.title_id
group by	authors.au_lname, authors.au_id --au_id needed because names are not unique
having		sum (titles.ytd_sales * titles.price * titles.royalty / 100 - titles.advance) < 0

--q6
select		type 'Book type',
			avg (price) 'Average price',
			min (price) 'Min price',
			max (price) 'Max price',
			sum (ytd_sales) 'YTD sales'
from		titles
where		year (pubdate) = 1991
group by	type
order by	avg (price) desc

--q7
select		stor_id 'Store ID',
			ord_date 'Order Date',
			sum (qty) 'Books sold'
from		sales
group by	stor_id, ord_date

--q8
select		title_id 'Title ID',
			count (*)
from		sales
group by	title_id
order by	count (*) desc

--q9
select		title_id 'Title ID',
			sum (qty) 'Books sold'
from		sales
group by	title_id
having		sum (qty) > 25
order by	sum (qty) desc

--q10
select		city 'City',
			state 'State',
			count (*) 'Authors'
from		authors
group by	city, state
having		count (*)  >= 2
order by	count (*) desc

--q11
select		type 'Book type',
			cast (avg (price) as decimal (20, 2)) 'Average price'
from		titles
where		title not like 'Computers%'
group by	type
having		avg (price) between 10 and 15

--75/78