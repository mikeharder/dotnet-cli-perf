package ClassLib123;

public class Class001 {
    public static String property() {
        return "ClassLib123" + " " + ClassLib100.Class001.property() + " " + ClassLib092.Class001.property();
    }
}
