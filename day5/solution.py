def getRow(line):
    floor = 1
    ceil = 128
    for i in range(7):
        if line[i] == 'F':
            ceil -= (ceil+1-floor)/2
        else:
            floor += (ceil+1-floor)/2
    return ceil-1

def getSeat(line):
    floor = 1
    ceil = 8
    for i in range(7,10):
        if line[i] == 'L':
            ceil -= (ceil+1-floor)/2
        else:
            floor += (ceil+1-floor)/2
    return ceil-1

def getID(line):
    row = getRow(line)
    seat = getSeat(line)
    return row*8 + seat

def getMissingID(ids):
    ids.sort()
    for seat in ids:
        if seat-1 not in ids and seat-2 in ids:
            return seat-1  
 
with open('input.txt') as f:
    ids = []
    for line in f:
        stripped = line.strip()
        ids.append(getID(line))
    
    print("Part one: %d" % (max(ids)))
    print("Part two: %d" % (getMissingID(ids)))