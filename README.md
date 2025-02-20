# **SimpScript Features**  

### **📝 Basic Commands**
- **`say [expression]`** → Prints a message to the console.  
  - Example: `say "Hello, World!"`  
- **`set [variable] = [value]`** → Assigns a value to a variable.  
  - Example: `set name = "Sunny"`  
- **`set [variable] = input "[prompt]"`** → Prompts user input and stores it in a variable.  
  - Example: `set name = input "Enter your name"`  

---

### **🔢 Expressions & Operations**
- Supports **string concatenation** with `+`.  
  - Example: `say "Hello, " + name`  
- Supports **math operations** (`+`, `-`, `*`, `/`).  
  - Example: `set x = 5 + 3`  
- Supports **variable replacement** in expressions.  
  - Example: `say x + 2` (if `x = 5`, prints `7`).  

---

### **🔁 Loops**
- **Fixed Loop:** `repeat [number] times` → Loops a set number of times.  
  - Example:  
    ```plaintext
    repeat 3 times
        say "Looping!"
    end
    ```
- **Conditional Loop:** `repeat while [condition]` → Loops while a condition is true.  
  - Example:  
    ```plaintext
    set x = 0
    repeat while x < 5
        say "x is " + x
        set x = x + 1
    end
    ```
- **Range Loop:** `repeat [var] from [start] to [end]` → Loops through a range.  
  - Example:  
    ```plaintext
    repeat i from 1 to 5
        say "Number " + i
    end
    ```

---

### **❓ Conditional Statements**
- **If Statements:** `if [condition]` → Executes code only if the condition is met.  
  - Example:  
    ```plaintext
    set x = 5
    if x > 3
        say "x is greater than 3"
    end
    ```
- **Supported Conditions:**  
  - `==` (equals)  
  - `!=` (not equals)  
  - `>` (greater than)  
  - `<` (less than)  
  - `>=` (greater than or equal to)  
  - `<=` (less than or equal to)  

---

### **🎯 Examples**
#### **Full Program**
```plaintext
say "What is your name?"
set name = input "Enter your name"
say "Hello, " + name

set x = 5
say x + " is set to " + x

if x > 3
    say x + " is greater than 3"
end

repeat 3 times
    say "Looping!"
end

repeat i from 1 to 3
    say "Counting: " + i
end
```
