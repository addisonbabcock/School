use pubs

--Q1
select	employee.fname 'First name',
		employee.lname 'Last name',
		publishers.pub_name 'Publisher'
from	employee
		join publishers
		on publishers.pub_id = employee.pub_id

--Q2
select	employee.fname 'First name',
		employee.lname 'Last name',
		publishers.pub_name 'Publisher'
from	employee, publishers
where	employee.pub_id = publishers.pub_id

--Q3
select	stores.stor_name 'Store Name',
		stores.city 'Store City',
		sales.qty 'Quantity'
from	stores
		join sales
		on sales.stor_id = stores.stor_id
where	stores.state like '%WA%' or stores.state like '%CA%'

--Q4
select	stores.stor_name 'Store Name',
		stores.city 'Store City',
		sales.qty 'Quantity'
from	stores, sales
where	(stores.state like '%WA%' or stores.state like '%CA%')
		and sales.stor_id = stores.stor_id

--Q5
select	employee.lname 'Last name',
		employee.fname 'First name',
		jobs.job_desc 'Job description',
		employee.hire_date 'Hire date'
from	employee
		join jobs
		on employee.job_id = jobs.job_id
where	employee.job_lvl = jobs.max_lvl
		and employee.emp_id like '%M'

--Q6
select	titles.type 'Book Type',
		titles.price 'Book Price',
		sales.qty 'Sales Quantity'
from	titles
		join sales
		on sales.title_id = titles.title_id
where	titles.advance between 1000 and 2000

--Q7
select	stores.stor_name 'Store name',
		stores.state 'Store state',
		sales.ord_date 'Order date',
		sales.qty 'Quantity sold'
from	stores
		join sales
		on sales.stor_id = stores.stor_id
where	(sales.qty < 15 or sales.qty > 25) and stores.zip like '8%'

--Q8
select	titles.title 'Title',
		titles.royalty 'Title Royalty',
		roysched.royalty 'Schedule royalty',
		roysched.lorange 'Low Range',
		roysched.hirange 'High Range'
from	titles, roysched
where	titles.title_id = roysched.title_id 
		and titles.ytd_sales between 8000 and 16000

--Q9
select	titles.title 'Title',
		titles.royalty 'Title Royalty',
		roysched.royalty 'Schedule royalty',
		roysched.lorange 'Low Range',
		roysched.hirange 'High Range'
from	titles, roysched
where	titles.title_id = roysched.title_id 
		and titles.ytd_sales between 8000 and 16000
		and titles.ytd_sales between roysched.lorange and roysched.hirange

--Q10
select	titles.type 'Book Type',
		convert (char (40), titles.title) 'Title',
		convert (char (18), authors.au_lname + ', ' + authors.au_fname) 'Author name'
from	titles
		join titleauthor
		on titles.title_id = titleauthor.title_id
				join authors
				on titleauthor.au_id = authors.au_id
where	authors.state like '%CA%'

--Q11
select	employee.fname + ' ' + employee.lname 'Employee Name'
from	employee, publishers, titles
where	employee.pub_id = publishers.pub_id
		and titles.pub_id = publishers.pub_id
		and titles.type like 'undecided'