import pyodbc
import random
from datetime import datetime, timedelta
from operator import itemgetter

conn = pyodbc.connect('Driver={SQL Server};'
                      'Server=localhost\SQLEXPRESS01;'
                      'Database=IKP;'
                      'Trusted_Connection=yes;')
cursor = conn.cursor()
cursor.execute('''select p.Product_Name, mc.Machine_Centre_Name, p.Min_Amount_Colour_Design, p.Min_Amount_Form
from Product p join Machine_Centre mc on p.Machine_Centre_Id=mc.Machine_Centre_Id''')

kk = []
for row in cursor:
    kk.append(row)
mc = []
for i in kk:
    l = []
    for k in i:
        if isinstance(k, str):
            l.append(k.strip())
        if not isinstance(k, str):
            l.append(k)
    if l[-2] == 0 and l[-1] == 0:
            l.pop(-1)
            l.pop(-1)
            l.append(4000)
            l.append(8000)
    mc.append(l)
dict = {}
for i in mc:
    if i[1] not in dict.keys():
        dict[i[1]] = [[i[0], i[2], i[3]]]
    else:
        dict[i[1]].append([i[0], i[2], i[3]])
print(dict)

cursor = conn.cursor()
cursor.execute('''select Machine_Centre_Name, maximum from
(select Machine_Centre_Id, max(ROUND(2592000/Cycle, 0, 1) * Gaps_Amount) as maximum from
(select p.Machine_Centre_Id, p.Cycle, p.PF_Id
from Product p) a
join (select pf.PF_Id, pf.Gaps_Amount from Press_Form pf) b on a.PF_Id = b.PF_Id
group by Machine_Centre_Id) c
join Machine_Centre mc on c.Machine_Centre_Id = mc.Machine_Centre_Id''')

kk = []
for row in cursor:
    kk.append(row)
maks = []
for i in kk:
    l = []
    for k in i:
        if isinstance(k, str):
            l.append(k.strip())
        if not isinstance(k, str):
            l.append(k)
    maks.append(l)

off = []
for i in dict:
    maximum = 0
    for k in maks:
        if i == k[0]:
            maximum = k[1]
    traffic = (random.randint(20, 50) / 100) * maximum
    c = 0
    while c < traffic:
        l = random.choice(dict[i])
        prod = random.choice([l[1], l[2]])
        plus = random.randint(prod, prod * 6)
        c += plus
        off.append([l[0], plus, i])
all1 = 0
for i in off:
    all1 += i[1]
all2 = 0
for i in maks:
    all2 += i[1]
for k in maks:
    c = 0
    for i in off:
        if i[-1] == k[0]:
            c += i[1] / k[1]
    print(str(round(c * 100, 2)) + '%')

print("_________________________________________")
print("Offers")
print(off)
cursor = conn.cursor()
cursor.execute('''select Product_Name, PF_Name, Machine_Centre_Name, Gaps_Amount, Cycle from Product p join Press_Form pf on p.PF_Id = pf.PF_Id
join Machine_Centre mc on mc.Machine_Centre_Id = p.Machine_Centre_Id''')
pr1 = []
for row in cursor:
    pr1.append(row)
pr = []
for i in pr1:
    l = []
    for k in i:
        if isinstance(k, str):
            l.append(k.strip())
        if not isinstance(k, str):
            l.append(k)
    pr.append(l)
print("Products:")
print(pr)


cursor = conn.cursor()
cursor.execute('''select Machine_Centre_Name, Date_Checked, Period, Time, Op_Name from MC_Maintenance m join Operation_Type o on m.Operation_Type_ID = o.Op_Type_Id 
join Machine_Centre mc on mc.Machine_Centre_Id = m.MC_Id''')
pr1 = []
for row in cursor:
    pr1.append(row)
to_m1 = []
for i in pr1:
    l = []
    for k in i:
        if isinstance(k, str):
            l.append(k.strip())
        if not isinstance(k, str):
            l.append(k)
    to_m1.append(l)
print("Machine TO:")
dict1 = {}
for i in to_m1:
    if i[0] not in dict1.keys():
        dict1[i[0]] = [[i[1], i[2], i[3], i[4]]]
    else:
        dict1[i[0]].append([i[1], i[2], i[3], i[4]])
print(dict1)

cursor = conn.cursor()
cursor.execute('''select PF_Name, Set_Time, Withdrawal_Time, Launch_Time, Colour_Change_Time from Press_Form''')
pr1 = []
for row in cursor:
    pr1.append(row)
to_m1 = []
for i in pr1:
    l = []
    for k in i:
        if isinstance(k, str):
            l.append(k.strip())
        if not isinstance(k, str):
            l.append(k)
    to_m1.append(l)

dict2 = {}
for i in to_m1:
    if i[0] not in dict2.keys():
        dict2[i[0]] = [[i[1], i[2], i[3], i[4]]]
    else:
        dict2[i[0]].append([i[1], i[2], i[3], i[4]])

cursor = conn.cursor()
cursor.execute('''select PF_Name, Date_Checked, Period, Time, Op_Name 
from PF_Maintenance m join Press_Form p on m.PF_Id=p.PF_Id join Operation_Type o on m.Operation_Type_ID=o.Op_Type_Id''')
pr1 = []
for row in cursor:
    pr1.append(row)
to_m1 = []
for i in pr1:
    l = []
    for k in i:
        if isinstance(k, str):
            l.append(k.strip())
        if not isinstance(k, str):
            l.append(k)
    to_m1.append(l)
for i in to_m1:
    for k in dict2:
        if i[0] == k:
            dict2[k].append([dict2[k][0], [i[1], i[2], i[3], i[4]]])
for i in dict2:
    dict2[i].pop(0)
print("TO Press form:")
print(dict2)
print("____________________________")
year = datetime.now().year
month = datetime.now().month
date = str(datetime.now().date())[-2:]
if date.startswith('0'):
    date = date[-1:]
now = datetime(year, month, int(date) + 1, 0, 0, 0, 0)
print(now)
fmt = '%Y-%m-%d %H:%M:%S'
Production = {}
for i in dict1:
    Production[i] = []
for i in dict1:
    for k in dict1[i]:
        if ((now - k[0]).seconds / 3600) > k[1]:
            if len(Production[i]) != 0:
                now = datetime.strptime(Production[i][-1][-1], fmt)
            Production[i].append([k[-2], k[-1], now.strftime(fmt), (now + timedelta(hours=k[-2])).strftime(fmt)])
del_it = []
for i in dict2:
    if len(dict2[i]) == 0:
        del_it.append(i)
for i in del_it:
    del dict2[i]

date_finished = []
for i in off:
    potential = []
    for k in pr:
        if k[0] == i[0]:
            potential.append(k)
    for l in potential:
        if len(l) == 5:
            pr_time = round((i[1] * l[-1]) / (60 * l[-2]), 0)
            l.append(pr_time)
    for p in potential:
        for m in Production:
            if p[2] == m and len(p) == 6:
                ending = datetime(year, month, int(date) + 1, 0, 0, 0, 0)
                if len(Production[m]) != 0:
                    ending = Production[m][-1][-1]
                ending = ending + timedelta(minutes=p[-1])
                p.append(ending)
    potential.sort(key=lambda x: x[-1])
    addition = []
    choice = potential[0]
    for ps in Production:
        if ps == choice[2]:
            if len(Production[ps]) != 0:
                last_press = Production[ps][-1][1]
                if choice[1] != last_press:
                    for pf in dict2:
                        if pf == last_press:
                            addition.append(['withdrawal', last_press, dict2[pf][0][0][0], Production[ps][-1][-1], Production[ps][-1][-1] + timedelta(minutes=dict2[pf][0][0][0])])
                            end = Production[ps][-1][-1] + timedelta(minutes=dict2[pf][0][0][0])
                    for pf in dict2:
                        if pf == choice[1]:
                            addition.append(['setting', last_press, dict2[pf][0][0][1], end, end + timedelta(minutes=dict2[pf][0][0][1])])
                            end = end + timedelta(minutes=dict2[pf][0][0][1])
                            addition.append(['running', last_press, dict2[pf][0][0][1], end, end + timedelta(minutes=dict2[pf][0][0][2])])
                if choice[1] == last_press:
                    for pf in dict2:
                        if pf == choice[1]:
                            addition.append(['running', last_press, dict2[pf][0][0][1], Production[ps][-1][-1], Production[ps][-1][-1] + timedelta(minutes=dict2[pf][0][0][2])])
            else:
                for pf in dict2:
                    if pf == choice[1]:
                        addition.append(['setting', choice[1], dict2[pf][0][0][1], datetime(year, month, int(date) + 1, 0, 0, 0, 0),
                                         datetime(year, month, int(date) + 1, 0, 0, 0, 0) + timedelta(minutes=dict2[pf][0][0][1])])
                        addition.append(['running', choice[1], dict2[pf][0][0][2], datetime(year, month, int(date) + 1, 0, 0, 0, 0) + timedelta(minutes=dict2[pf][0][0][1]),
                                         datetime(year, month, int(date) + 1, 0, 0, 0, 0) + timedelta(minutes=dict2[pf][0][0][1]) + timedelta(minutes=dict2[pf][0][0][2])])
    to_needed = []
    for d1 in dict1:
        if choice[2] == d1:
            for t in dict1[d1]:
                if (t[0] + timedelta(hours=t[1])) < choice[-1]:
                    to_needed.append(t)
    for d2 in dict2:
        if choice[1] == d2:
            for t in dict2[d2]:
                if (t[1][0] + timedelta(hours=t[1][1])) < choice[-1]:
                    to_needed.append(t)
    new_to = []
    for it in to_needed:
        if len(it) == 2:
            new_to.append(['Pr', it[1][-1], it[1][-2] * 60, it[1][0] + timedelta(hours=it[1][1]), it[1][0] + timedelta(hours=it[1][1]) + timedelta(minutes=it[1][2] * 60)])
        else:
            new_to.append(['M', it[-1], it[-2] * 60, it[0] + timedelta(hours=it[1]), it[0] + timedelta(hours=it[1]) + timedelta(minutes=it[2] * 60)])
    new_to.sort(key=lambda x: x[-2])
    for proc in new_to:
        if len(addition) == 0:
            start = choice[-1] + timedelta(minutes=choice[-2] * (-1))
            if start > proc[-2]:
                addition.append([proc[0], proc[1], proc[2], start, start + timedelta(minutes=proc[2])])

            else:
                diff = round((proc[-2] - start).seconds / 60, 0)
                addition.append([choice[0], choice[1], choice[2], diff, start, start + timedelta(minutes=diff)])
                addition.append([proc[0], proc[1], proc[2], addition[-1][-1], addition[-1][-1] + timedelta(minutes=proc[2])])
            if proc[0] == 'Pr':
                for press in dict2:
                    if choice[1] == press:
                        for spec in dict2[press]:
                            if spec[1][-1] == proc[1]:
                                spec[1].pop(0)
                                spec[1].insert(0, start + timedelta(minutes=proc[2]))
            else:
                for mc in dict1:
                    if mc == choice[2]:
                        for spec in dict1[mc]:
                            if spec[-1] == proc[1]:
                                spec.pop(0)
                                spec.insert(0, start + timedelta(minutes=proc[2]))
        elif (addition[-1][0] == 'M' or addition[-1][0] == 'Pr') and addition[-1][-1] > proc[-2]:
            addition.append([proc[0], proc[1], proc[2], addition[-1][-1], addition[-1][-1] + timedelta(minutes=proc[2])])
            if proc[0] == 'Pr':
                for press in dict2:
                    if choice[1] == press:
                        for spec in dict2[press]:
                            if spec[1][-1] == proc[1]:
                                spec[1].pop(0)
                                spec[1].insert(0, addition[-1][-1] + timedelta(minutes=proc[2]))
            else:
                for mc in dict1:
                    if mc == choice[2]:
                        for spec in dict1[mc]:
                            if spec[-1] == proc[1]:
                                spec.pop(0)
                                spec.insert(0, addition[-1][-1] + timedelta(minutes=proc[2]))
        else:
            diff = round((proc[-2] - addition[-1][-1]).seconds / 60, 0)
            addition.append([choice[0], choice[1], choice[2], diff, addition[-1][-1], addition[-1][-1] + timedelta(minutes=diff)])
            addition.append([proc[0], proc[1], proc[2], addition[-1][-1], addition[-1][-1] + timedelta(minutes=proc[2])])
            if proc[0] == 'Pr':
                for press in dict2:
                    if choice[1] == press:
                        for spec in dict2[press]:
                            if spec[1][-1] == proc[1]:
                                spec[1].pop(0)
                                spec[1].insert(0, addition[-1][-1] + timedelta(minutes=proc[2]))
            else:
                for mc in dict1:
                    if mc == choice[2]:
                        for spec in dict1[mc]:
                            if spec[-1] == proc[1]:
                                spec.pop(0)
                                spec.insert(0, addition[-1][-1] + timedelta(minutes=proc[2]))
    count = 0
    for each in addition:
        if each[0] != 'M' and each[0] != 'Pr' and not each[0].startswith('s') and not each[0].startswith('w') and not each[0].startswith('r'):
            count += each[3]
    if choice[5] - count > 0:
        if len(addition) == 0:
            start = choice[-1] + timedelta(minutes=choice[-2] * (-1))
            addition.append([choice[0], choice[1], choice[2], choice[5], start, choice[-1]])
        else:
            addition.append([choice[0], choice[1], choice[2], choice[5] - count, addition[-1][-1], addition[-1][-1] + timedelta(minutes=(choice[5] - count))])
    for machine in Production:
        if choice[2] == machine:
            for slot in addition:
                Production[machine].append(slot)
    date_finished.append([i[0], i[1], addition[-1][-1]])

for i in Production:
    for k in Production[i]:
        if len(k) == 6:
            k.pop(2)
print(Production)
for k in Production:
    file = k.replace('/', '_') + ".txt"
    f = open(file, "w")
    for i in Production[k]:
        f.write(str(i[0]) + " " + str(i[1]) + "," + str(i[2]) + "," + str(i[3]) + "," + str(i[4]) + '\n')
    f.close()
file = "отгрузка.txt"
f = open(file, "w")
for i in date_finished:
    f.write(str(i[0] + ',' + str(i[1]) + ',' + str(i[2]) + '\n'))



