def decrypt(card, door):
    loops = 1
    while True:
        if pow(7, loops, 20201227) == card:
            return pow(door, loops, 20201227)
        loops += 1

with open("example.txt") as f:
    keys = [int(i) for i in f.readlines()]

print("part one: %d" % decrypt(keys[0], keys[1]))