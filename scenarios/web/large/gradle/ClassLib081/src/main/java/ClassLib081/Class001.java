package ClassLib081;

public class Class001 {
    public static String property() {
        return "ClassLib081" + " " + ClassLib021.Class001.property() + " " + ClassLib003.Class001.property() + " " + ClassLib051.Class001.property() + " " + ClassLib052.Class001.property() + " " + ClassLib027.Class001.property() + " " + ClassLib075.Class001.property();
    }
}
