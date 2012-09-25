/* Make a new database (unless it already exists!) called "Ex2Practice" 
and use it as THE CURRENT DB FOR ALL QUESTIONS.

1. List the names of all Publishers who employ people who are managers.
Only list any specific publisher once! */

use pubs

select distinct pub_name 'Publisher'
from pubs..publishers p
	join pubs..employee e
	on e.pub_id = p.pub_id
		join pubs..jobs j
		on j.job_id = e.job_id
where j.job_desc like '%manag%'

/* 2. List the names of all books that have been ordered from each store.  Also list
store name... ie, results like this:

Store Name= 1st col
Book Name=2nd col 

Only include books that are priced over $5.  */ 

select	st.stor_name 'Store Name',
		ti.title 'Book Name'
from	pubs..stores st
		join	pubs..sales sa
		on		sa.stor_id = st.stor_id
				join	pubs..titles ti
				on		ti.title_id = sa.title_id
where	ti.price > 5

/* 3. Given that an order for "x" copies
of a specific book = 1 record in sales table:

Return:

i) Book Name
ii) number of different orders for "x" copies of the book. 

Limit your results to books priced over $5
that have appeared on more than 1 different order.
*/

select	titles.title 'Book Name',
		count (*) 'Number of orders'
from	titles
		join	sales
		on		sales.title_id = titles.title_id
where titles.price > 5
group by titles.title, titles.title_id
having count (*) > 1

/* 4. List the name(s) of author(s) (formatted as John Doe) who have written
at least one book that are priced over $20. Include the number of books they
have written. */

select	authors.au_fname + ' ' + authors.au_lname 'Name',
		count (titleauthor.au_id) 'Books written'
from	authors
		join	titleauthor	on	titleauthor.au_id = authors.au_id
				join	titles	on	titleauthor.title_id = titles.title_id
where	titles.price > 20
group by authors.au_fname, authors.au_lname, authors.au_id

/* 5. List the name of each store and number of orders placed on Sept 14/94 ONLY */

select	stores.stor_name 'Store',
		count (ord_num) 'Orders placed'
from	stores
		join	sales	on	sales.stor_id = stores.stor_id
where	sales.ord_date = 'Sep 14, 1994'
group by stores.stor_id, stores.stor_name

/* 6. Use Northwind for source data.  Retrieve the Company Name of each supplier
and the no. of products they supply that are priced in the range of $15-$40. Limit
your results to companies supplying more than 2 products meeting these requirements.*/

use northwind

select	suppliers.companyname 'Company Name',
		count (*) 'Number of products'
from	suppliers
		join	products	
		on	suppliers.supplierid = products.supplierid
where	products.unitprice between 15 and 40
group by suppliers.companyname, suppliers.supplierid
having	count (*) > 2