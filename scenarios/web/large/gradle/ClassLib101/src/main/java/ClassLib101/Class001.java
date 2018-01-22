package ClassLib101;

public class Class001 {
    public static String property() {
        return "ClassLib101" + " " + ClassLib001.Class001.property() + " " + ClassLib092.Class001.property();
    }
}
