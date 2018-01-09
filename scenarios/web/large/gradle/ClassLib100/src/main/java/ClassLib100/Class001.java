package ClassLib100;

public class Class001 {
    public static String property() {
        return "ClassLib100" + " " + ClassLib001.Class001.property() + " " + ClassLib091.Class001.property();
    }
}
