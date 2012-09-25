use pubs

select		sales.stor_id 'Store ID',
			stor_name 'Store Name',
			min (qty) 'Min sales QTY',
			max (qty) 'Max sales QTY',
			avg (qty) 'Average QTY'
from		sales
	join	stores
	on		stores.stor_id = sales.stor_id
where		sales.stor_id < 7100
group by	sales.stor_id, stor_name
having		sales.stor_id < 7100
order by	stor_name