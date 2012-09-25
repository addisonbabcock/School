use northwind

--columns must be aggregate or specified in group by clause
select	employeeid 'Employee ID',
		count (*) 'Order count', --counts rows in a group
		min (OrderDate) 'Earliest order date',
		avg (Freight) 'Average freight cost'
from orders
where employeeid in (1, 2, 3, 4, 5)
group by employeeid
--if it is possible to use a where then do so
having avg (Freight) > 80
order by min (OrderDate), employeeid --any column from the group by clause or any columns in select list
