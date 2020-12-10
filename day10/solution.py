# TIL about dynamic programming. Thanks, Reddit!
def countWays(line, ways):
    ways[line] = max(
        sum(
            ways.get(i, 0) for i in range(
                max(line-3, 0),
                line
            )
        ),
        1
    )

def getLines(file):
    with open(file, 'r') as f:
        lines = sorted([int(x) for x in f])
        lines.insert(0, 0) # Outlet
        lines.append(lines[-1] +3) # Device

        return lines

prev = 0
diffs = {}
ways = {0:1}
for line in getLines('example.txt'):
    diff = line - prev
    diffs[diff] = diffs.get(diff, 0) +1
    
    countWays(line, ways)
    
    prev = line

print("Part one: %d" % (diffs[1] * diffs[3]))
print("Part two: %d" % ways[max(ways)])